using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard
{
    public class VendorDashBoardDetailsDto
    {
        public int VendorCount { get; set; }
        public List<DashBoardVendorCountDto> TypeCount { get; set; } = [];
    }
}
