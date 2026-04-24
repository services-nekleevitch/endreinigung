namespace EndReinigung.Services
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(string toEmail, string subject, string htmlBody, string? replyTo = null);
    }
}
