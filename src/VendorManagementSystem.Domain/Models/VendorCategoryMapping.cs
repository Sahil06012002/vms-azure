using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorManagementSystem.Models.Models
{
    public class VendorCategoryMapping
    {
        public int Id { get; set; }
        [ForeignKey("VendorNew")]
        public int? VendorId { get; set; }
        [Required]
        public VendorNew? Vendor { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        [Required]
        public Category? Category { get; set; }

        public bool Status { get; set; } = true;
    }

}
