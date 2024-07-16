using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice
{
    public class SalesInvoiceEmailDto
    {
        public int BuyingVendor { get; set; }
        public int SellingVendor { get; set; }
        public string InvoiceId { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public String Terms { get; set; } = String.Empty;
        public DateTime? DueDate { get; set; }
        public string PlaceOfSupply { get; set; } = string.Empty;
        public List<SaleItem> Items { get; set; } = [];
        public decimal AmountPaid { get; set; }

        public string? Subject { get; set; } = string.Empty;
        public string? Body { get; set; } = string.Empty;
    }
}
