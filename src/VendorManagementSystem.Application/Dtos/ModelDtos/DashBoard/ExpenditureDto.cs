using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard
{
    public class ExpenditureDto
    {
        public decimal TotalExpenditure { get; set; }
        public List<CategoryExpenditureDto> CategoryExpenditures { get; set; } = [];
    }
}
