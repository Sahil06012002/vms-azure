using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice
{
    public class SalesInvoiceResponseDto
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; 
        public decimal Amount { get; set; }

    }
}
