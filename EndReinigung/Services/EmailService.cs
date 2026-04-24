using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace EndReinigung.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendContactEmailAsync(string toEmail, string subject, string htmlBody, string? replyTo = null)
        {
            using var client = new SmtpClient(_settings.Server, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.UseSSL || _settings.UseStartTls
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                mail.ReplyToList.Add(new MailAddress(replyTo));
            }

            await client.SendMailAsync(mail);
        }
    }
}
