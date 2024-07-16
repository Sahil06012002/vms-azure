using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IVendorService
    {
        public ApplicationResponseDto<int> CreateVendor(CreateVendorNewDto vendornNewDto, string jwtToken);
        ApplicationResponseDto<PagenationResponseDto> GetAllVendors(string? filter, int cursor, int size, bool next);
        public ApplicationResponseDto<bool> UpdateVendor(UpdateVendorNewDto updateDto, string jwtToken);
        ApplicationResponseDto<VendorNewResponseDto> GetVendorById(int vendorId);

        ApplicationResponseDto<bool> ToogleVendorStatus(int vendorId,string jwtToken);

        ApplicationResponseDto<VendorFormDataDto> GetFormData();
    }
}
