    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VendorManagementSystem.Models.Enums;

    namespace VendorManagementSystem.Models.Models
    {
        public class SalesInvoice
        {
            public int Id { get; set; }
            public int? CreatorId { get; set; }
            public int? VendorId { get; set; }
            public int DestinationId { get; set; }

            [Column(TypeName = "varchar(100)")]
            public string? Reference { get; set; }
            public DateTime? Date { get; set; }
            public DateTime? DueDate { get; set; }
            public PurchaseStatus Status { get; set; }
            public PaymentTerms PaymentTerms { get; set; }

            [Column(TypeName = "decimal(12,3)")]
            public decimal Amount { get; set; }

            [Column(TypeName = "decimal(12,3)")]
            public decimal AmountPaid { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
            public int CreatedBy { get; set; }
            public int UpdatedBy { get; set; }
            [ForeignKey("CreatedBy")]
            public User? Creator { get; set; }

            [ForeignKey("UpdatedBy")]
            public User? Updater { get; set; }

            [ForeignKey(nameof(CreatorId))]
            public VendorNew? SalesInvoiceCreator { get; set; }

            [ForeignKey(nameof(VendorId))]
            public VendorNew? Vendor { get; set; }

            [ForeignKey(nameof(DestinationId))]
            public State? DestinationState { get; set; }
    }
        
    }
