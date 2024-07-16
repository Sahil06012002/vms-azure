using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.Contract
{
    public class ContractDto
    {
        [Required]
        public int VendorCategoryId { get; set; }
        [Required]
        public string ContactPersonName { get; set; } = string.Empty;
        [Required]
        public string ContactPersonEmail { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
        [Required]
        public string PaymentMode { get; set; } = string.Empty;
        [Required]
        public int Status { get; set; }
        // file upload
        public IFormFile? File { get; set; }

    }
}
