using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder
{
    public class PurchaseOrderFormDto
    {
        public  int Id { get; set; }
        public IEnumerable<object> Vendor { get; set; } = [];
        public string Identifier { get; set; } = string.Empty;
        public IEnumerable<State> States { get; set; } = [];
        public string[] PaymentTerms { get; set; } = [];
        public IEnumerable<object> addresses { get; set; } = [];


    }
}
