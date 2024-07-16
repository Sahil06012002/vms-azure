using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder
{
    public  class PurchaseEmailDto
    {
        public int DelivaryTo { get; set; }
        public int DelivaryFrom { get; set; }
        public string? EmailBody { get; set; } = string.Empty;
        public string? EmailSubject { get; set; } = string.Empty;
        public PdfGenerationDto? PdfGenerationDto { get; set; }
    }
}
