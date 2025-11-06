namespace AppServices
{
    public interface IEmailService
    {
        Task SendBookingConfirmationAsync(string to, string subject, string htmlBody);
        void SendBookingConfirmation(string to, string subject, string htmlBody);
    }
}