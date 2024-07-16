using System.ComponentModel.DataAnnotations;

namespace VendorManagementSystem.Application.Dtos.ModelDtos
{
    public class UpdatePasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
