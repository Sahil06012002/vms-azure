

namespace VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice
{
    public class SaleItem
    {
        public string ItemName { get; set; } = string.Empty;
        public string HSN { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public float CGST { get; set; }
        public float SGST { get; set; }
        public decimal Amount { get; set; }
    }
}
