using System.Text;
using EndReinigung.Models;
using EndReinigung.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EndReinigung.Controllers
{
    [Route("calculator")]
    public class CalculatorController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly SmtpSettings _smtp;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(
            IEmailService emailService,
            IOptions<SmtpSettings> smtpOptions,
            ILogger<CalculatorController> logger)
        {
            _emailService = emailService;
            _smtp = smtpOptions.Value;
            _logger = logger;
        }

        [HttpPost("book")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([FromForm] BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kv => kv.Value?.Errors.Count > 0)
                    .ToDictionary(kv => kv.Key, kv => kv.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(new { ok = false, errors });
            }

            // Authoritative server price — never trust client value for anything sensitive.
            var authoritativePrice = PriceCalculator.Compute(model);
            if (authoritativePrice <= 0m)
            {
                return BadRequest(new { ok = false, errors = new { RoomSize = new[] { "Ungültige Wohnungsgrösse." } } });
            }

            _logger.LogInformation(
                "Booking: {FirstName} {LastName} <{Email}> {Property}/{Rooms} ZIP {Zip} -> CHF {Price} (client quoted {Quoted})",
                model.FirstName, model.LastName, model.Email, model.PropertyType, model.RoomSize,
                model.ZipCode, authoritativePrice, model.QuotedTotal);

            var recipient = !string.IsNullOrWhiteSpace(_smtp.RecipientEmail) ? _smtp.RecipientEmail : _smtp.SenderEmail;
            if (string.IsNullOrWhiteSpace(recipient))
            {
                _logger.LogError("Calculator booking: no recipient email configured.");
                return StatusCode(500, new { ok = false, error = "Email configuration missing." });
            }

            var bookingId = $"ZR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
            var subject = $"Buchung {bookingId} — {model.FirstName} {model.LastName} — CHF {authoritativePrice:N0}";
            var body = BuildBookingEmail(model, authoritativePrice, bookingId);

            try
            {
                await _emailService.SendContactEmailAsync(recipient, subject, body, model.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Calculator booking email failed for {BookingId}", bookingId);
                return StatusCode(500, new { ok = false, error = "Buchung konnte nicht gesendet werden." });
            }

            return Ok(new
            {
                ok = true,
                bookingId,
                total = authoritativePrice,
                cleaningDate = model.CleaningDate?.ToString("yyyy-MM-dd"),
            });
        }

        private static string BuildBookingEmail(BookingViewModel m, decimal total, string bookingId)
        {
            var sb = new StringBuilder();
            sb.Append($"<h2 style=\"font-family:sans-serif;\">Neue Buchung {Enc(bookingId)}</h2>");
            sb.Append("<h3 style=\"font-family:sans-serif;\">Objekt</h3>");
            sb.Append("<table style=\"border-collapse:collapse;font-family:sans-serif;font-size:14px;\">");
            Row(sb, "Objekttyp", m.PropertyType == "house" ? "Haus" : "Wohnung");
            Row(sb, "Grösse", $"{m.RoomSize} Zimmer");
            Row(sb, "PLZ", m.ZipCode);
            sb.Append("</table>");

            sb.Append("<h3 style=\"font-family:sans-serif;margin-top:16px;\">Extras</h3>");
            sb.Append("<table style=\"border-collapse:collapse;font-family:sans-serif;font-size:14px;\">");
            if (m.Basement) Row(sb, "Estrich", $"+CHF {PriceCalculator.PriceBasement:N0}");
            if (m.Balcony > 0) Row(sb, $"Extra Balkon / Terrasse ({m.Balcony}×)", $"+CHF {m.Balcony * PriceCalculator.PriceBalcony:N0}");
            if (m.UtilityBalcony > 0) Row(sb, $"Nebenbalkon ({m.UtilityBalcony}×)", $"+CHF {m.UtilityBalcony * PriceCalculator.PriceUtilityBalcony:N0}");
            if (m.Bath > 0) Row(sb, $"Anzahl Badezimmer ({m.Bath}×)", $"+CHF {m.Bath * PriceCalculator.PriceBath:N0}");
            if (m.Wc > 0) Row(sb, $"Separates WC ({m.Wc}×)", $"+CHF {m.Wc * PriceCalculator.PriceWc:N0}");
            if (m.Carpet > 0) Row(sb, $"Teppich Shampoo-Reinigung ({m.Carpet} Zimmer)", $"+CHF {m.Carpet * PriceCalculator.PriceCarpet:N0}");
            if (m.BalconyPressure > 0) Row(sb, $"Balkon / Terrasse Hochdruckreinigung ({m.BalconyPressure}×)", $"+CHF {m.BalconyPressure * PriceCalculator.PriceBalconyPressure:N0}");
            if (m.GaragePressure > 0) Row(sb, $"Garage / Carport Hochdruckreinigung ({m.GaragePressure}×)", $"+CHF {m.GaragePressure * PriceCalculator.PriceGaragePressure:N0}");
            sb.Append("</table>");

            sb.Append("<h3 style=\"font-family:sans-serif;margin-top:16px;\">Termine</h3>");
            sb.Append("<table style=\"border-collapse:collapse;font-family:sans-serif;font-size:14px;\">");
            Row(sb, "Reinigungstermin", m.CleaningDate?.ToString("dd.MM.yyyy") ?? "-");
            if (m.HandoverDateNotFixed)
            {
                Row(sb, "Übergabe", "Datum noch nicht fix");
            }
            else
            {
                Row(sb, "Übergabetermin", m.HandoverDate?.ToString("dd.MM.yyyy") ?? "-");
                Row(sb, "Übergabezeit", m.HandoverTime ?? "-");
            }
            sb.Append("</table>");

            sb.Append("<h3 style=\"font-family:sans-serif;margin-top:16px;\">Kundendaten</h3>");
            sb.Append("<table style=\"border-collapse:collapse;font-family:sans-serif;font-size:14px;\">");
            Row(sb, "Name", Enc($"{m.FirstName} {m.LastName}"));
            Row(sb, "E-Mail", Enc(m.Email));
            Row(sb, "Telefon", Enc(m.Phone));
            Row(sb, "Adresse", Enc($"{m.CustomerStreet}, {m.CustomerPlz} {m.CustomerCity}"));
            var objStreet = m.SameAsCustomer ? m.CustomerStreet : m.ObjectStreet;
            var objPlz = m.SameAsCustomer ? m.CustomerPlz : m.ObjectPlz;
            var objCity = m.SameAsCustomer ? m.CustomerCity : m.ObjectCity;
            Row(sb, "Objektadresse", Enc($"{objStreet}, {objPlz} {objCity}"));
            Row(sb, "Zahlungsmethode", Enc(m.PaymentMethod));
            if (!string.IsNullOrWhiteSpace(m.Notes))
            {
                Row(sb, "Anmerkungen", Enc(m.Notes).Replace("\n", "<br>"));
            }
            sb.Append("</table>");

            sb.Append($"<h3 style=\"font-family:sans-serif;margin-top:16px;color:#0a7a2f;\">Total: CHF {total:N0}</h3>");
            if (total != m.QuotedTotal)
            {
                sb.Append($"<p style=\"color:#b45309;font-family:sans-serif;font-size:13px;\">Hinweis: Clientpreis ({m.QuotedTotal:N0}) weicht vom Serverpreis ab.</p>");
            }
            return sb.ToString();
        }

        private static void Row(StringBuilder sb, string label, string value)
        {
            sb.Append("<tr><td style=\"padding:4px 14px 4px 0;font-weight:bold;vertical-align:top;\">");
            sb.Append(Enc(label));
            sb.Append("</td><td style=\"padding:4px 0;\">");
            sb.Append(value);
            sb.Append("</td></tr>");
        }

        private static string Enc(string s) => System.Net.WebUtility.HtmlEncode(s ?? "");
    }
}
