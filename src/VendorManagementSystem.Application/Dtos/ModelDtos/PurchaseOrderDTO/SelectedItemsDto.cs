using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO
{
    public class SelectedItemsDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string HSN { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public int Tax { get; set; }
        public decimal Amount { get; set; }
    }
}
