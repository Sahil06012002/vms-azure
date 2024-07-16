using System.ComponentModel.DataAnnotations.Schema;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Models.Models
{
    public class VendorNew
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public PaymentTerms PaymentTerms { get; set; }
        public int TDSId { get; set; }
        public int VendorTypeId { get; set; }
        public string FileName { get; set; } = string.Empty;

        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public PrimaryContact? Contacts { get; set; }

        public VendorTdsOption? TDS { get; set; }

        public VendorType? VendorType { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public User? Creator { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public User? Updater { get; set; }
    }
}
