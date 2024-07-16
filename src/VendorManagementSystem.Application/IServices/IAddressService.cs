using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IAddressService
    {
        public ApplicationResponseDto<IEnumerable<object>> GetVendorAddress(int vendorId);
    }
}
