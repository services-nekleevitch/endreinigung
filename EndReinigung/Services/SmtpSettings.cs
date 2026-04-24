namespace EndReinigung.Services
{
    public class SmtpSettings
    {
        public string Server { get; set; } = "";
        public int Port { get; set; }
        public string SenderName { get; set; } = "";
        public string SenderEmail { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool UseSSL { get; set; }
        public bool UseStartTls { get; set; }
        public string RecipientEmail { get; set; } = "";
    }
}
