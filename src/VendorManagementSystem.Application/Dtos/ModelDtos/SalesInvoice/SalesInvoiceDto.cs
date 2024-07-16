using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice
{
    public class SalesInvoiceDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int VendorId { get; set; }
        public int DestinationId { get; set; }
        public string? Reference { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DueDate { get; set; }
        public string PaymentTerms { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public List<SelectedItemsDto> selectedItems { get; set; } = [];

        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}