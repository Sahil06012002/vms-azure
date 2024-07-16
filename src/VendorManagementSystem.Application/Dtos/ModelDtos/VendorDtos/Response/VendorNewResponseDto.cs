namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response
{
    public class VendorNewResponseDto
    {
        public int VendorId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string PaymentTerms { get; set; } = string.Empty;
        public string TDS { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Status { get; set; }
        public List<string> Categories { get; set; } = [];
        public PrimaryContactResponseDto? PrimaryContact { get; set; }
        public AddressResponseDto? ShippingAddress { get; set; }
        public AddressResponseDto? BillingAddress { get; set; }
    }
}
