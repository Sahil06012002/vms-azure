using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.Item
{
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string Account { get; set; } = string.Empty;
        public int Gst { get; set; }
        public int IGst { get; set; }
        public string HSN { get; set; } = string.Empty;
    }
}
