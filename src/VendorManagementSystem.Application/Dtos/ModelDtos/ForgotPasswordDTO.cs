using System.ComponentModel.DataAnnotations;

namespace VendorManagementSystem.Application.Dtos.ModelDtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Url]
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
