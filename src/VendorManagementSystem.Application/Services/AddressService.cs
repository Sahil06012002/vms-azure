using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public  class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public ApplicationResponseDto<IEnumerable<object>> GetVendorAddress(int vendorId)
        {
            try
            {
                var address = _addressRepository.RelatedAddresses([vendorId]);
                return new ApplicationResponseDto<IEnumerable<object>>
                {
                    Data = address
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponseDto<IEnumerable<object>>
                {
                    Error = new()
                    {
                        Message = [ex.Message]
                    }
                };
            }
        }
    }
}
