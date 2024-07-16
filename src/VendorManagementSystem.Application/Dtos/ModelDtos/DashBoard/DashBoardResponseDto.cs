using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard
{
    public class DashBoardResponseDto
    {
        public VendorDashBoardDetailsDto? VendorDetails { get; set; }
        public ExpenditureDto? Expenditure { get; set; }
    }
}
