namespace VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder
{
    public class PurchaseOrderUtilityResponseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Identifier { get; set; } = string.Empty;  
        public string? Reference { get; set; } =string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? ExpectedDileveryDate { get; set; }


    }
}
