using System.ComponentModel.DataAnnotations.Schema;

namespace VendorManagementSystem.Models.Models
{
    public class Unit
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "varchar(20)")]
        public string Code { get; set; } = string.Empty;
    }
}
