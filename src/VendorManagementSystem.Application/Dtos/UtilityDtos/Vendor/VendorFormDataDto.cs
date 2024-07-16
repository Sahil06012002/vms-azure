using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor
{
    public class VendorFormDataDto
    {
        public IEnumerable<VendorFormCategoriesDto>? Categories { get; set; }
        public IEnumerable<VendorFormTypesDto>? VednorTypes { get; set; }
        public IEnumerable<string> Salutations { get; set; } = [];
        public IEnumerable<string> Currency { get; set; } = [];
        public IEnumerable<string> AddressTypes { get; set; } = [];
        public IEnumerable<string> Country { get; set; } = [];
        public IEnumerable<string> PaymentTerms { get; set; } = [];
        public IEnumerable<State> States { get; set; } = [];
        public IEnumerable<VendorTdsOption> TDSOptions { get; set; } = [];

    }
}
