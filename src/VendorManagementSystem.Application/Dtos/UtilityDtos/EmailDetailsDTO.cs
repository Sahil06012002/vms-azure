namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class EmailDetailsDto
    {
        //string fromName, string toName, string toAddress, string link
        public string ToAddress { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}
