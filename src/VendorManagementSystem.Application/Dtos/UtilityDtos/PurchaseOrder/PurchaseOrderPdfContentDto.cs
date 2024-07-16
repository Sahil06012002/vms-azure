using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder
{
    public class PurchaseOrderPdfContentDto
    {
        public string Name { get; set; } = string.Empty;
        public byte[] PdfBytes { get; set; } = [];
    }
}
