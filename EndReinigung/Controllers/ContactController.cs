using System.Text;
using EndReinigung.Models;
using EndReinigung.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EndReinigung.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly SmtpSettings _smtp;
        private readonly ILogger<ContactController> _logger;

        public ContactController(
            IEmailService emailService,
            IOptions<SmtpSettings> smtpOptions,
            ILogger<ContactController> logger)
        {
            _emailService = emailService;
            _smtp = smtpOptions.Value;
            _logger = logger;
        }

        [HttpPost("send")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(ContactFormViewModel model)
        {
            var returnUrl = SafeReturnUrl(model.ReturnUrl);

            _logger.LogInformation(
                "Contact form submission: {FirstName} {LastName} <{Email}> phone={Phone} obj={Objektart} rooms={Rooms} address={Address}",
                model.FirstName, model.LastName, model.Email, model.Phone, model.Objektart, model.Rooms, model.Address);

            if (!ModelState.IsValid)
            {
                TempData["Contact.Status"] = "invalid";
                return Redirect(returnUrl + "#contact-form");
            }

            var recipient = !string.IsNullOrWhiteSpace(_smtp.RecipientEmail)
                ? _smtp.RecipientEmail
                : _smtp.SenderEmail;

            if (string.IsNullOrWhiteSpace(recipient))
            {
                _logger.LogError("Contact form: no recipient email configured (SmtpSettings.RecipientEmail / SenderEmail empty).");
                TempData["Contact.Status"] = "error";
                return Redirect(returnUrl + "#contact-form");
            }

            var subject = $"Neue Anfrage von {model.FirstName} {model.LastName}";
            var body = BuildEmailBody(model);

            try
            {
                await _emailService.SendContactEmailAsync(recipient, subject, body, model.Email);
                TempData["Contact.Status"] = "success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Contact form email send failed");
                TempData["Contact.Status"] = "error";
            }

            return Redirect(returnUrl + "#contact-form");
        }

        private string SafeReturnUrl(string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return "/";
            }
            return returnUrl;
        }

        private static string BuildEmailBody(ContactFormViewModel m)
        {
            var sb = new StringBuilder();
            sb.Append("<h2 style=\"font-family:sans-serif;\">Neue Anfrage von der Website</h2>");
            sb.Append("<table style=\"border-collapse:collapse;font-family:sans-serif;font-size:14px;\">");
            Row(sb, "Name", Enc(m.FirstName + " " + m.LastName));
            Row(sb, "E-Mail", Enc(m.Email));
            Row(sb, "Telefon", Enc(m.Phone ?? "-"));
            Row(sb, "Adresse", Enc(m.Address));
            Row(sb, "Zimmer", Enc(m.Rooms ?? "-"));
            Row(sb, "Auszugsdatum", m.MoveOut.HasValue ? m.MoveOut.Value.ToString("yyyy-MM-dd") : "-");
            Row(sb, "Objektart", Enc(m.Objektart));
            Row(sb, "Stockwerk", Enc(m.Stockwerk));
            Row(sb, "Möblierung", Enc(m.Moebelierung));
            Row(sb, "Nachricht", Enc(m.Message ?? "-").Replace("\n", "<br>"));
            sb.Append("</table>");
            return sb.ToString();
        }

        private static void Row(StringBuilder sb, string label, string value)
        {
            sb.Append("<tr><td style=\"padding:6px 14px;font-weight:bold;vertical-align:top;\">");
            sb.Append(Enc(label));
            sb.Append("</td><td style=\"padding:6px 14px;\">");
            sb.Append(value);
            sb.Append("</td></tr>");
        }

        private static string Enc(string s) => System.Net.WebUtility.HtmlEncode(s);
    }
}
