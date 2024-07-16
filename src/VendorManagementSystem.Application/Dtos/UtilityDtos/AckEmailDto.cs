using VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class AckEmailDto
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string ToEmailAddress { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string FromEmailAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public FileContentDto Pdf { get; set; } = new FileContentDto();
    }
}
