using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.Item
{
    public class ItemFormDto
    {
        public string[] TaxPreference { get; set; } = [];
        public List<int> GstRates { get; set; } = [];
        public IEnumerable<Unit> Units { get; set; } = [];
    }
}
