
using System.ComponentModel.DataAnnotations.Schema;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Models.Models
{
    public class PrimaryContact
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public Salutation? Salutation { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? WorkPhone { get; set; }
        public string? MobilePhone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        // constraints

        [ForeignKey(nameof(VendorId))]
        public VendorNew? Vendor { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public User? Creator { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public User? Updator { get; set; }
    }
}
