

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorManagementSystem.Models.Models
{
    public class UserToken
    {
        [Column(TypeName = "varchar(100)")]
        public String Email { get; set; } = string.Empty;
        [Column(TypeName = "varchar(500)")]
        public String Token { get; set; } = string.Empty;
    }
}
