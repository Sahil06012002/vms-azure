using System.ComponentModel.DataAnnotations.Schema;

namespace VendorManagementSystem.Models.Models
{
    public class User
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string UserName { get; set; } = string.Empty;
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; } = string.Empty;
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; } = string.Empty;
        [Column(TypeName = "varchar(50)")]
        public string Role { get; set; } = string.Empty;
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
