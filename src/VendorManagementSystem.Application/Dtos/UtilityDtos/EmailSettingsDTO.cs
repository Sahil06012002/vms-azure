namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class EmailSettingsDto
    {
        public string From { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string AuthEmail { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;
        public string SmtpClient { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
