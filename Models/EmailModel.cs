namespace AutoSendEmail.Models
{
    public class EmailModel
    {
        public string FromEmail { get; set; } = string.Empty;
        public string ToEmail { get; set; } = string.Empty;
        public string CCEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}