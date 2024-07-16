namespace VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int VendorId { get; set; }
        public int CustomerId { get; set; }
        public int SourceStateId { get; set; }
        public int DestinationStateId { get; set; }
        public string? Reference { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string PaymentTerms { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PurchaseStatus { get; set; } = string.Empty;

        public List<SelectedItemsDto> Items { get; set; } = [];
    }
}
