namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response
{
    public class AddressResponseDto
    {
        public int AddressId { get; set; }
        public string Attention { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;     
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FaxNumber { get; set; } = string.Empty;
    }
}
