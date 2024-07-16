namespace VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder
{
    public class PdfGenerationDto
    {
        public int CreatorId { get; set; }
        public int DelivaryTo { get; set; }
        public int DelivaryFrom { get; set; }
        public DateOnly Date { get; set; }
        public string PurchaseOrderId { get; set; } = string.Empty;
        public List<ItemsRow> Rows { get; set; } = [];
        public decimal SubTotal { get; set; }
        public decimal GST { get; set; }
    }
}
