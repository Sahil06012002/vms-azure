using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder
{
    public class PurchaseOrderEmailDto
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string ToEmailAddress { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string FromEmailAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public PurchaseOrderPdfContentDto? Pdf { get; set; }
    }
}
