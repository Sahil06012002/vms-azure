using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace VendorManagementSystem.Application.Dtos.ModelDtos.Invoice
{
    public class CreateInvoiceDto
    {
        [Required]
        public int VendorCategoryMappingId { get; set; }
        public string ContactPersonName { get; set; } = string.Empty;
        public string ContactPersonEmail { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        public DateOnly DueDate { get; set; }
        public bool Status { get; set; } = false;
        public IFormFile? file { get; set; }

    }
}
