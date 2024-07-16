using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.SalesInvoice
{
    public class SalesInvoiceFormDto
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public IEnumerable<State> States { get; set; } = [];
        public IEnumerable<object> Vendors { get; set; } = [];
        public IEnumerable<object> Addresses { get; set; } = [];
        public string[] PaymentTerms { get; set; } = [];
        public string[] Status { get; internal set; } = [];
    }
}
