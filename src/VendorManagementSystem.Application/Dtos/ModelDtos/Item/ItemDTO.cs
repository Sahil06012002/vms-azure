using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.Item
{
    public  class ItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string TaxPreference { get; set; } = string.Empty;
        public decimal SellingPrice { get; set; }
        public string SalesAccount { get; set; } = string.Empty;
        public string? SalesDescription { get; set; }
        public decimal CostPrice { get; set; }
        public string PurchaseAccount { get; set; } = string.Empty;
        public string? PurchaseDescription { get; set; }
        public int GstRate { get; set; }
        public int IGstRate { get; set; }
    }
}





