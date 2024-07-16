using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DataContext _db;
        public InvoiceRepository(DataContext db)
        {
            _db = db;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public int AddInvoice(Invoice invoice)
        {
            _db.Add(invoice);
            _db.SaveChanges();
            return invoice.Id;
        }
        public List<InvoiceResponseDto> GetInvoices(string? filter, int cursor, int size, bool next)
        {
            IQueryable<Invoice> query = _db.Invoices;



            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(i =>
                    i.VendorCategoryMapping!.Vendor!.CompanyName.Contains(filter) ||
                    i.VendorCategoryMapping!.Category!.Name.Contains(filter)
                );
            }


            if (cursor == 0)
            {
                query = query.OrderByDescending(i => i.Id).Take(size);
            }
            else if (next)
            {
                query = query.OrderByDescending(i => i.Id).Where(i => i.Id < cursor).Take(size);
            }
            else
            {
                query = query.OrderBy(i => i.Id).Where(i => i.Id > cursor).Take(size);
            }

            var result = query.Select(i => new InvoiceResponseDto()
            {
                Id = i.Id,
                OrganizationName = i.VendorCategoryMapping!.Vendor!.CompanyName,
                CategoryName = i.VendorCategoryMapping!.Category!.Name,
                Amount = i.Amount,
                ContactPersonEmail = i.ContactPersonEmail,
                ContactPersonName = i.ContactPersonName,
                DueDate = i.DueDate,
                Status = i.Status,
                FileName = i.FileName
            });

            return next ? result.ToList() : result.OrderByDescending(r => r.Id).ToList();
        }
        public CountDto GetInvoiceCount()
        {
            
            var statusCount = _db.Invoices.GroupBy(i => i.Status).Select(g=> new { Status = g.Key, StatusCount = g.Count() }).ToList();
            int activeCount = 0;
            int inactiveCount = 0;

            foreach (var item in statusCount)
            {
                if (item.Status)
                {
                    activeCount = item.StatusCount;
                }
                else
                {
                    inactiveCount = item.StatusCount;
                }
            }
            CountDto result = new CountDto()
            {
                active = activeCount,
                inactive = inactiveCount,
            };
            return result;
        }
        public bool neighbourExistance(string filter, int id, bool next)
        {
            IQueryable<Invoice> query = _db.Invoices;
            if(!filter.IsNullOrEmpty())
            {
                query = _db.Invoices.Where(i => filter == null ||
                   (i.VendorCategoryMapping!.Vendor!.CompanyName.Contains(filter) ||
                    i.VendorCategoryMapping!.Category!.Name.Contains(filter)));
            }

            query =  query.OrderByDescending(i => i.Id);
            var hasNeighbour = next
                ? query.Any(i => i.Id < id)
                : query.Any(i => i.Id > id);
            return hasNeighbour;
        }

        public ExpenditureDto GetTopCategories(int count)
        {
            var query = _db.Invoices.Where(invoice => invoice.Status).GroupBy(invoice => invoice.VendorCategoryMapping!.Category, Invoice => Invoice.Amount, (category, amount) => new CategoryExpenditureDto { 
                Name=category!=null?category.Name:"Unknown Category", 
                Expenditure=amount.Sum().GetValueOrDefault() 
            }).OrderByDescending(a => a.Expenditure).Take(count);
            return new ExpenditureDto
            {
                CategoryExpenditures = query.ToList(),
                TotalExpenditure = _db.Invoices.Where(invoice => invoice.Status).Sum(invoice => invoice.Amount).GetValueOrDefault()
            };
        }

    }
}
