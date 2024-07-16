using System.ComponentModel.DataAnnotations.Schema;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Models.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;
        public int? UnitId { get; set; } 
        public ItemType ItemType { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Code { get; set; }
        public TaxPreference TaxPreference { get; set; }
        [Column(TypeName = "decimal(10,3)")]
        public decimal SellingPrice { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string SalesAccount { get; set; } = string.Empty;
        [Column(TypeName = "varchar(255)")]
        public string? SalesDescription { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string PurchaseAccount { get; set; } = string.Empty;

        [Column(TypeName = "varchar(255)")]
        public string? PurchaseDescription { get; set; }
        public int GstRate { get; set; }
        public int IGstRate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public User? Creator { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? Updater { get; set; }
        public Unit? Unit { get; set; }

    }
}
