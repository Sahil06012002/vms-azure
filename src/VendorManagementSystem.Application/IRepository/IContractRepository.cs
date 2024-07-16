using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Contract;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IContractRepository
    {
        public int AddContract(Contract contract);
        public IEnumerable<ContractResponseDto> GetContracts(PaginationDto paginationDto, string? filter);
        public Contract? GetContracts(int id);
        public IDbContextTransaction BeginTransaction();
        public IEnumerable<ContractStatus> GetContractStatus();
        public bool NeighbourExsistance(int id, bool next);
    }
}
