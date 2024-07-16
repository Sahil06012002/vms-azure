using System.ComponentModel.DataAnnotations;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos
{
    public class CreateVendorDto
    {
        [Required]
        public string OrganizationName { get; set; } = string.Empty;
        [Required]
        public int VendorTypeId { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string ContactPersonName { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string ContactPersonNumber { get; set; } = string.Empty;
        [Required]
        public string ContactPersonEmail { get; set; } = string.Empty;
        public string RelationshipDuration { get; set; } = string.Empty;
        public List<int> CategoryIds { get; set; } = [];
    }
}