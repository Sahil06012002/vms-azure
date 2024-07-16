using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard
{
    public class CategoryExpenditureDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Expenditure { get; set; }
    }
}
