using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class InvoiceResponseDto
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string ContactPersonEmail { get; set; } = string.Empty;
        public string ContactPersonName { get; set; } = string.Empty;
        public DateOnly DueDate { get; set; }
        public bool Status { get; set; }
        public string FileName { get; set; } = string.Empty;
    }
}
