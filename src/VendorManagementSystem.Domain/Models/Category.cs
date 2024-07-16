
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorManagementSystem.Models.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "varchar(255)")]
        public string? Description { get; set; }
        public bool Status { get; set; }    
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public User? Creator { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? Updater { get; set; }

    }
}
