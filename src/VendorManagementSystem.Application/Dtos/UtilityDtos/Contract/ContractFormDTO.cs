using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.Contract
{
    public class ContractFormDto
    {
        public IEnumerable<ContractStatus>? ContractStatus { get; set; }
        public IEnumerable<object>? Vendor { get; set; }
        // contains a object with id and vendor name
    }
}
