using VendorManagementSystem.Application.Dtos.ModelDtos.Contract;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Contract;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IContractService
    {
        public Task<ApplicationResponseDto<Contract>> AddContract(ContractDto contractDto, string token);

        public ApplicationResponseDto<ContractFormDto> GetContractFormCreationData();
        public ApplicationResponseDto<IEnumerable<object>> GetVendorCategories(int id);
        public Task<ApplicationResponseDto<FileContentDto>> GetFile(string name);
        public ApplicationResponseDto<PagenationResponseDto> GetAllContracts(PaginationDto paginationDto, string? filter);
        //public ApplicationResponseDto<PagenationResponseDto> GetAllContracts(int lastId, int pageSize, string filter);
    }
}
