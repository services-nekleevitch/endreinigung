using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace EndReinigung.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> options, ILogger<EmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendContactEmailAsync(string toEmail, string subject, string htmlBody, string? replyTo = null)
        {
            // Correlates all lines of one send attempt in the log file.
            var sendId = Guid.NewGuid().ToString("N")[..8];
            var sw = Stopwatch.StartNew();

            LogConfigurationProblems(sendId);

            _logger.LogInformation(
                "Email {SendId}: sending via {Server}:{Port} (ssl={Ssl}, starttls={StartTls}, user={User}) from {From} to {To} replyTo={ReplyTo} subject=\"{Subject}\" bodyLength={BodyLength}",
                sendId, _settings.Server, _settings.Port, _settings.UseSSL, _settings.UseStartTls, _settings.Username,
                _settings.SenderEmail, toEmail, replyTo ?? "-", subject, htmlBody?.Length ?? 0);

            MailMessage mail;
            try
            {
                mail = new MailMessage
                {
                    From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                if (!string.IsNullOrWhiteSpace(replyTo))
                {
                    try
                    {
                        mail.ReplyToList.Add(new MailAddress(replyTo));
                    }
                    catch (FormatException ex)
                    {
                        // A malformed customer address must not block the ops notification.
                        _logger.LogWarning(ex, "Email {SendId}: reply-to address {ReplyTo} is not a valid address — sending without Reply-To", sendId, replyTo);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email {SendId}: could not build message (to={To}, from={From}) — invalid address?", sendId, toEmail, _settings.SenderEmail);
                throw;
            }

            using (mail)
            using (var client = new SmtpClient(_settings.Server, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.UseSSL || _settings.UseStartTls,
                Timeout = 30_000
            })
            {
                try
                {
                    await client.SendMailAsync(mail);
                    _logger.LogInformation("Email {SendId}: sent OK to {To} in {ElapsedMs} ms", sendId, toEmail, sw.ElapsedMilliseconds);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    var rejected = string.Join(", ", ex.InnerExceptions.Select(e => $"{e.FailedRecipient} ({e.StatusCode})"));
                    _logger.LogError(ex, "Email {SendId}: server rejected recipient(s) {Rejected} after {ElapsedMs} ms", sendId, rejected, sw.ElapsedMilliseconds);
                    throw;
                }
                catch (SmtpFailedRecipientException ex)
                {
                    _logger.LogError(ex, "Email {SendId}: server rejected recipient {Recipient} with {StatusCode} after {ElapsedMs} ms", sendId, ex.FailedRecipient, ex.StatusCode, sw.ElapsedMilliseconds);
                    throw;
                }
                catch (SmtpException ex)
                {
                    _logger.LogError(ex, "Email {SendId}: SMTP failure {StatusCode} talking to {Server}:{Port} after {ElapsedMs} ms (auth/TLS/relay problem — see inner exception)", sendId, ex.StatusCode, _settings.Server, _settings.Port, sw.ElapsedMilliseconds);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Email {SendId}: unexpected failure sending to {To} after {ElapsedMs} ms", sendId, toEmail, sw.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        private void LogConfigurationProblems(string sendId)
        {
            if (string.IsNullOrWhiteSpace(_settings.Server))
                _logger.LogError("Email {SendId}: SmtpSettings.Server is empty", sendId);
            if (_settings.Port <= 0)
                _logger.LogError("Email {SendId}: SmtpSettings.Port is {Port}", sendId, _settings.Port);
            if (string.IsNullOrWhiteSpace(_settings.Username) || string.IsNullOrWhiteSpace(_settings.Password))
                _logger.LogWarning("Email {SendId}: SMTP credentials incomplete (username set={HasUser}, password set={HasPassword})",
                    sendId, !string.IsNullOrWhiteSpace(_settings.Username), !string.IsNullOrWhiteSpace(_settings.Password));
            if (string.IsNullOrWhiteSpace(_settings.SenderEmail))
                _logger.LogError("Email {SendId}: SmtpSettings.SenderEmail is empty", sendId);
        }
    }
}
