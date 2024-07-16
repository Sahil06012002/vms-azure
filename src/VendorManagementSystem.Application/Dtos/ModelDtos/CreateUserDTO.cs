using System.ComponentModel.DataAnnotations;

namespace VendorManagementSystem.Application.Dtos.ModelDtos
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty ;
        [Required]
        public string Role { get; set; } = string.Empty;
        [Required]
        [Url]
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
