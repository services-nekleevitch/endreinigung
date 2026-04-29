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

            var bookingNumber = $"ZR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            _logger.LogInformation(
                "Booking {BookingNumber}: {FirstName} {LastName} <{Email}> {Property}/{Rooms} ZIP {Zip} -> CHF {Price}",
                bookingNumber, model.FirstName, model.LastName, model.Email, model.PropertyType, model.RoomSize,
                model.ZipCode, authoritativePrice);

            // DB persistence is disabled for now; rely entirely on email until re-enabled.
            // Ops notification to info@
            var opsRecipient = !string.IsNullOrWhiteSpace(_smtp.RecipientEmail) ? _smtp.RecipientEmail : _smtp.SenderEmail;
            if (!string.IsNullOrWhiteSpace(opsRecipient))
            {
                var subject = $"Buchung {bookingNumber} — {model.FirstName} {model.LastName} — CHF {authoritativePrice:N0}";
                var body = BuildOpsNotificationEmail(model, authoritativePrice, bookingNumber);
                try
                {
                    await _emailService.SendContactEmailAsync(opsRecipient, subject, body, model.Email);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ops notification email failed for {BookingNumber}", bookingNumber);
                    return StatusCode(500, new { ok = false, error = "Buchung konnte nicht gesendet werden. Bitte versuchen Sie es erneut." });
                }
            }
            else
            {
                _logger.LogError("Calculator booking: no recipient email configured.");
                return StatusCode(500, new { ok = false, error = "Email configuration missing." });
            }

            // Customer confirmation — best-effort
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var custSubject = $"Ihre Buchungsbestätigung {bookingNumber} — Zürich Endreinigung";
                var custBody = BuildCustomerConfirmationEmail(model, authoritativePrice, bookingNumber);
                try
                {
                    await _emailService.SendContactEmailAsync(model.Email, custSubject, custBody);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Customer confirmation email failed for {BookingNumber}", bookingNumber);
                    // Do not fail the booking — ops already got their copy.
                }
            }

            return Ok(new
            {
                ok = true,
                bookingId = bookingNumber,
                total = authoritativePrice,
                cleaningDate = model.CleaningDate?.ToString("yyyy-MM-dd"),
            });
        }

        private static string BuildOpsNotificationEmail(BookingViewModel m, decimal total, string bookingId)
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

        private static string BuildCustomerConfirmationEmail(BookingViewModel m, decimal total, string bookingNumber)
        {
            var propertyLabel = m.PropertyType == "house" ? "Haus" : "Wohnung";
            var cleaningDate = m.CleaningDate?.ToString("dd.MM.yyyy") ?? "wird bestätigt";

            var sb = new StringBuilder();
            sb.Append("<div style=\"font-family:-apple-system,Segoe UI,Roboto,sans-serif;max-width:560px;margin:0 auto;color:#1a1a1a;\">");
            sb.Append("<div style=\"background:linear-gradient(135deg,#1e4a8a,#2a66b8);padding:28px 32px;color:white;border-radius:8px 8px 0 0;\">");
            sb.Append("<h1 style=\"margin:0;font-size:22px;font-weight:700;\">Vielen Dank für Ihre Buchung!</h1>");
            sb.Append($"<p style=\"margin:8px 0 0;opacity:0.9;font-size:14px;\">Buchungsnummer: <strong>{Enc(bookingNumber)}</strong></p>");
            sb.Append("</div>");

            sb.Append("<div style=\"padding:28px 32px;background:#ffffff;border:1px solid #e5e7eb;border-top:none;\">");
            sb.Append($"<p>Hallo {Enc(m.FirstName)} {Enc(m.LastName)},</p>");
            sb.Append("<p>wir haben Ihre Anfrage erhalten. Ein Mitarbeiter meldet sich innerhalb von 24 Stunden mit der Terminbestätigung bei Ihnen.</p>");

            sb.Append("<h2 style=\"font-size:16px;margin:24px 0 12px;color:#1e4a8a;\">Ihre Buchung im Überblick</h2>");
            sb.Append("<table style=\"border-collapse:collapse;width:100%;font-size:14px;\">");
            Row(sb, "Objekt", $"{propertyLabel} • {Enc(m.RoomSize)} Zimmer");
            Row(sb, "Adresse", Enc($"{m.CustomerStreet}, {m.CustomerPlz} {m.CustomerCity}"));
            Row(sb, "Reinigungstermin", cleaningDate);
            if (!m.HandoverDateNotFixed && m.HandoverDate.HasValue)
            {
                var handover = m.HandoverDate.Value.ToString("dd.MM.yyyy");
                if (!string.IsNullOrWhiteSpace(m.HandoverTime)) handover += $", {Enc(m.HandoverTime)}";
                Row(sb, "Übergabe", handover);
            }
            Row(sb, "Zahlungsart", PaymentLabel(m.PaymentMethod));
            sb.Append("</table>");

            var hasExtras = m.Basement || m.Balcony > 0 || m.UtilityBalcony > 0 || m.Bath > 0 || m.Wc > 0
                            || m.Carpet > 0 || m.BalconyPressure > 0 || m.GaragePressure > 0;
            if (hasExtras)
            {
                sb.Append("<h2 style=\"font-size:16px;margin:24px 0 12px;color:#1e4a8a;\">Gewählte Extras</h2>");
                sb.Append("<table style=\"border-collapse:collapse;width:100%;font-size:14px;\">");
                if (m.Basement) Row(sb, "Estrich", $"CHF {PriceCalculator.PriceBasement:N0}");
                if (m.Balcony > 0) Row(sb, $"Extra Balkon / Terrasse ({m.Balcony}×)", $"CHF {m.Balcony * PriceCalculator.PriceBalcony:N0}");
                if (m.UtilityBalcony > 0) Row(sb, $"Nebenbalkon ({m.UtilityBalcony}×)", $"CHF {m.UtilityBalcony * PriceCalculator.PriceUtilityBalcony:N0}");
                if (m.Bath > 0) Row(sb, $"Zusätzliche Badezimmer ({m.Bath}×)", $"CHF {m.Bath * PriceCalculator.PriceBath:N0}");
                if (m.Wc > 0) Row(sb, $"Separates WC ({m.Wc}×)", $"CHF {m.Wc * PriceCalculator.PriceWc:N0}");
                if (m.Carpet > 0) Row(sb, $"Teppich-Shampoo ({m.Carpet} Zimmer)", $"CHF {m.Carpet * PriceCalculator.PriceCarpet:N0}");
                if (m.BalconyPressure > 0) Row(sb, $"Balkon Hochdruckreinigung ({m.BalconyPressure}×)", $"CHF {m.BalconyPressure * PriceCalculator.PriceBalconyPressure:N0}");
                if (m.GaragePressure > 0) Row(sb, $"Garage Hochdruckreinigung ({m.GaragePressure}×)", $"CHF {m.GaragePressure * PriceCalculator.PriceGaragePressure:N0}");
                sb.Append("</table>");
            }

            sb.Append("<div style=\"margin-top:24px;padding:16px;background:#ecfdf5;border-left:4px solid #0a7a2f;border-radius:4px;\">");
            sb.Append($"<div style=\"font-size:13px;color:#065f2b;margin-bottom:4px;\">Fixpreis inkl. MwSt. &amp; Abnahmegarantie</div>");
            sb.Append($"<div style=\"font-size:24px;font-weight:700;color:#065f2b;\">CHF {total:N0}</div>");
            sb.Append("</div>");

            sb.Append("<h2 style=\"font-size:16px;margin:24px 0 12px;color:#1e4a8a;\">Was passiert jetzt?</h2>");
            sb.Append("<ol style=\"padding-left:20px;margin:0;font-size:14px;line-height:1.6;\">");
            sb.Append("<li>Wir prüfen Ihre Anfrage und melden uns innerhalb von 24 h mit der Terminbestätigung.</li>");
            sb.Append("<li>Am Reinigungstag erscheint unser Team pünktlich vor Ort.</li>");
            sb.Append("<li>Bei der Übergabe sind wir persönlich dabei — und reinigen bei Beanstandungen kostenlos nach.</li>");
            sb.Append("</ol>");

            sb.Append("<div style=\"margin-top:28px;padding-top:20px;border-top:1px solid #e5e7eb;font-size:13px;color:#6b7280;\">");
            sb.Append("<p style=\"margin:0 0 8px;\"><strong>Fragen?</strong> Antworten Sie einfach auf diese E-Mail oder rufen Sie uns an:</p>");
            sb.Append("<p style=\"margin:0;\">📞 <a href=\"tel:+41765017738\" style=\"color:#1e4a8a;text-decoration:none;\">+41 76 501 77 38</a> &nbsp;·&nbsp; ✉️ <a href=\"mailto:info@zurich-endreinigung.ch\" style=\"color:#1e4a8a;text-decoration:none;\">info@zurich-endreinigung.ch</a></p>");
            sb.Append("</div>");

            sb.Append("<p style=\"margin-top:24px;font-size:13px;color:#6b7280;\">Herzliche Grüsse<br><strong>Ihr Team von Zürich Endreinigung</strong></p>");
            sb.Append("</div></div>");
            return sb.ToString();
        }

        private static string PaymentLabel(string method) => method switch
        {
            "twint" => "TWINT",
            "bank" => "Banküberweisung (Vorauszahlung)",
            "cash" => "Barzahlung bei Übergabe",
            _ => method ?? "-"
        };

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
