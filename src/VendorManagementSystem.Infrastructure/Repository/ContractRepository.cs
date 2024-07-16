using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Contract;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class ContractRepository : IContractRepository
    {
        private readonly DataContext _db;

        public ContractRepository(DataContext db)
        {
            _db = db;
        }
        public int AddContract(Contract contract)
        {
            _db.Contracts.Add(contract);
            int res = _db.SaveChanges();
            return res;
        }

        public Contract? GetContracts(int id)
        {
            Contract? contract = _db.Contracts.Where(contract => contract.Id == id).FirstOrDefault();
            return contract;
        }

        public IEnumerable<ContractResponseDto> GetContracts(PaginationDto paginationDto, string? filter)
        {
            int cursor = paginationDto.Cursor, pageSize = paginationDto.Size;
            bool next = paginationDto.Next;
            
            IQueryable<Contract> query;

            if (next)
            {
                if (cursor == 0)
                {
                    query = _db.Contracts.OrderByDescending(c => c.Id);
                }
                else
                {
                    query = _db.Contracts.OrderByDescending(contract => contract.Id).Where(contract => contract.Id < cursor);
                }
            }
            else
            {
                query = _db.Contracts.OrderBy(contract => contract.Id).Where(contract => contract.Id > cursor);
            }

            if(!string.IsNullOrWhiteSpace(filter))
            {
                query = query
                    .Where(contract => contract.VendorCategoryMapping!=null &&(contract.VendorCategoryMapping.Vendor!=null && contract.VendorCategoryMapping.Vendor.CompanyName.Contains(filter) || contract.VendorCategoryMapping.Category!=null && contract.VendorCategoryMapping.Category.Name.Contains(filter)));
            }

           var response = query
                .Take(pageSize)
                .Select(contract => new ContractResponseDto
                {
                    Id = contract.Id,
                    OrganizationName = contract.VendorCategoryMapping!=null && contract.VendorCategoryMapping.Vendor!=null?contract.VendorCategoryMapping.Vendor.CompanyName:string.Empty,
                    CategoryName = contract.VendorCategoryMapping != null && contract.VendorCategoryMapping.Category != null ? contract.VendorCategoryMapping.Category.Name:string.Empty,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    PaymentMode = contract.PaymentMode,
                    Status = contract.ContractStatus!=null?contract.ContractStatus.Name:string.Empty,
                    FileName = contract.FileName!=null?contract.FileName:string.Empty,
                    Amount = contract.Amount,
                    ContactPersonEmail = contract.ContactPersonEmail,
                    ContactPersonName = contract.ContactPersonName
                }).ToList();

            if(next) return response;
            return response.OrderByDescending(c => c.Id);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public IEnumerable<ContractStatus> GetContractStatus()
        {
            var contractStatus = _db.ContractStatus.ToList();
            return contractStatus;
        }

        public bool NeighbourExsistance(int id, bool next)
        {
            IQueryable<Contract> query = _db.Contracts.OrderByDescending(x => x.Id);
            return next ? query.Any(c => c.Id < id) : query.Any(c => c.Id > id);
        }
    }
}
