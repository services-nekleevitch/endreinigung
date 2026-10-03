using System.Collections.Concurrent;
using System.Text;

namespace EndReinigung.Services
{
    /// <summary>
    /// Options for <see cref="FileLoggerProvider"/>, bound from the "FileLogging" config section.
    /// </summary>
    public class FileLoggerOptions
    {
        /// <summary>Directory (relative to content root or absolute) that receives one file per day.</summary>
        public string Path { get; set; } = "App_Data/logs";

        /// <summary>Minimum level written to the file. Category filters from "Logging" still apply on top.</summary>
        public LogLevel MinLevel { get; set; } = LogLevel.Information;

        /// <summary>Files older than this are deleted on startup and once a day.</summary>
        public int RetainDays { get; set; } = 30;
    }

    /// <summary>
    /// Minimal, dependency-free rolling file logger. The hosting environment (IIS on hostfactory) does not
    /// persist console output, so anything the controllers/EmailService log is otherwise lost — this is
    /// what lets us diagnose failed bookings after the fact.
    /// One file per day: App_Data/logs/app-YYYY-MM-DD.log. Writes are serialized through a single
    /// background queue so concurrent requests never interleave lines.
    /// </summary>
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _directory;
        private readonly LogLevel _minLevel;
        private readonly int _retainDays;
        private readonly BlockingCollection<string> _queue = new(new ConcurrentQueue<string>());
        private readonly Thread _writer;
        private DateTime _lastCleanup = DateTime.MinValue;

        public FileLoggerProvider(string directory, LogLevel minLevel, int retainDays)
        {
            _directory = directory;
            _minLevel = minLevel;
            _retainDays = retainDays;
            Directory.CreateDirectory(_directory);
            _writer = new Thread(WriteLoop) { IsBackground = true, Name = "FileLogger" };
            _writer.Start();
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(this, categoryName);

        internal bool IsEnabled(LogLevel level) => level >= _minLevel && level != LogLevel.None;

        internal void Enqueue(string line)
        {
            if (!_queue.IsAddingCompleted) _queue.Add(line);
        }

        private void WriteLoop()
        {
            foreach (var line in _queue.GetConsumingEnumerable())
            {
                try
                {
                    var now = DateTime.Now;
                    var file = System.IO.Path.Combine(_directory, $"app-{now:yyyy-MM-dd}.log");
                    File.AppendAllText(file, line, Encoding.UTF8);
                    if ((now - _lastCleanup).TotalHours >= 24)
                    {
                        _lastCleanup = now;
                        Cleanup(now);
                    }
                }
                catch
                {
                    // Logging must never take the site down. If the disk is full or the folder is
                    // read-only there is nothing useful we can do here.
                }
            }
        }

        private void Cleanup(DateTime now)
        {
            foreach (var f in Directory.EnumerateFiles(_directory, "app-*.log"))
            {
                try
                {
                    if (File.GetLastWriteTime(f) < now.AddDays(-_retainDays)) File.Delete(f);
                }
                catch { /* best effort */ }
            }
        }

        public void Dispose()
        {
            _queue.CompleteAdding();
            _writer.Join(TimeSpan.FromSeconds(2));
        }

        private sealed class FileLogger : ILogger
        {
            private readonly FileLoggerProvider _provider;
            private readonly string _category;

            public FileLogger(FileLoggerProvider provider, string category)
            {
                _provider = provider;
                _category = category;
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => _provider.IsEnabled(logLevel);

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel)) return;

                var sb = new StringBuilder(256);
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                  .Append(" [").Append(Abbrev(logLevel)).Append("] ")
                  .Append(_category).Append(": ")
                  .Append(formatter(state, exception));

                // Walk the full exception chain — SmtpException usually wraps the real socket/TLS error.
                var depth = 0;
                for (var ex = exception; ex != null; ex = ex.InnerException, depth++)
                {
                    sb.AppendLine().Append(new string(' ', 4 * (depth + 1)))
                      .Append(depth == 0 ? "" : "--> ")
                      .Append(ex.GetType().FullName).Append(": ").Append(ex.Message);
                    if (ex is System.Net.Mail.SmtpException smtp)
                    {
                        sb.Append(" [SmtpStatusCode=").Append(smtp.StatusCode).Append(']');
                    }
                    if (ex is System.Net.Sockets.SocketException sock)
                    {
                        sb.Append(" [SocketError=").Append(sock.SocketErrorCode).Append(']');
                    }
                    if (depth == 0 && !string.IsNullOrEmpty(ex.StackTrace))
                    {
                        sb.AppendLine().Append(ex.StackTrace);
                    }
                }
                sb.AppendLine();
                _provider.Enqueue(sb.ToString());
            }

            private static string Abbrev(LogLevel l) => l switch
            {
                LogLevel.Trace => "TRC",
                LogLevel.Debug => "DBG",
                LogLevel.Information => "INF",
                LogLevel.Warning => "WRN",
                LogLevel.Error => "ERR",
                LogLevel.Critical => "CRT",
                _ => l.ToString()
            };
        }
    }
}
