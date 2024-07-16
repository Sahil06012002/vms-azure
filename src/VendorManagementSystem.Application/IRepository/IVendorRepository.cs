using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IVendorRepository
    {
        IDbContextTransaction BeginTransaction();
        public int AddVendor(VendorNew vendor);

        public int AddVendorAddress(Address address);
        public bool AddVendorAddress(List<Address> addresses);
        public int AddVendorPrimaryContact(PrimaryContact primaryContact);
        VendorNewResponseDto? GetVendorById(int vendorId);
        
        public bool UpdateVendor(int vendorId, List<UpdateColumnDto> vendorColumns, string currentUser);
        public bool UpdateAddress(int vendorId, List<UpdateColumnDto> addressColumns, AddressTypes addressType, int currentUser);
        public bool UpdatePrimaryContact(int vendorId, List<UpdateColumnDto> primaryContactColumns, int currentUser);
        bool ToggleStatus(int vendorId,int currentUser);
        public CountDto GetVendorCount();
        IEnumerable<VendorFormTypesDto> GetTypesForForm();
        public IEnumerable<object> VednorFormDetails();
        public bool HasNeighbour(string filter,int id, bool next);
        public IEnumerable<VendorNewResponseDto> GetVendorsNew(string? filter, int cursor, int size, bool next);
        public Address? GetVendorAddress(int vendorId, AddressTypes type);
        public VendorDashBoardDetailsDto GetVendorDashBoradData();
    }
}
