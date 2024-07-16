using System.ComponentModel.DataAnnotations.Schema;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Models.Models
{
    public class PurchaseOrder
    {

        public int Id { get; set; }
        public int? CreatorId { get; set; } //purchase order creator Id
        public int? VendorId { get; set; }
        public int? CustomerId { get; set; }//dilevery address
        public int? SourceStateId { get; set; }//could also be stored as a string , retrieval would be easy
        public int? DestinationStateId { get; set; }//same as above

        [Column(TypeName = "varchar(100)")]
        public string?  Reference { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public PaymentTerms PaymentTerms { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal Amount { get; set; }
        public PurchaseStatus PurchaseStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User? Creator { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public User? Updater { get; set; }
        [ForeignKey(nameof(VendorId))]
        public VendorNew? Vendor { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public VendorNew? Organization { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public VendorNew? POCreator { get; set; }
        [ForeignKey(nameof(SourceStateId))]
        public State? SourceState { get; set; }
        [ForeignKey(nameof(DestinationStateId))]
        public State? DestinationState { get; set; }

    }
}
