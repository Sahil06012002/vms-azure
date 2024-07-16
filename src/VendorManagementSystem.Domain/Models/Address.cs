using System.ComponentModel.DataAnnotations.Schema;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Models.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public AddressTypes? AddressType { get; set; }
        public string? Attention { get; set; }
        public Country? Country { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public int? StateId { get; set; }
        public string? PinCode { get; set; }
        public string? Phone { get; set; }
        public string? FaxNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }



        [ForeignKey(nameof(VendorId))]
        public VendorNew? Vendor { get; set; }

        public State? state { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public User? Creator { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public User? Updator { get; set; }
    }
}
