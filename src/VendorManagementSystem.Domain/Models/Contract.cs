
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VendorManagementSystem.Models.Models
{
    public class Contract
    {
        public int Id { get; set; }

        public int VendorCategoryMappingId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ContactPersonName { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string ContactPersonEmail { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,3)")]
        public decimal Amount { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PaymentMode { get; set; } = string.Empty;

        public int ContractStatusId { get; set; }

        // file upload
        [Column(TypeName = "varchar(255)")]
        public string? FileName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public User? Creator { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? Updater { get; set; }

        public VendorCategoryMapping? VendorCategoryMapping { get; set; } 
        public ContractStatus? ContractStatus { get; set; }

    }
}
