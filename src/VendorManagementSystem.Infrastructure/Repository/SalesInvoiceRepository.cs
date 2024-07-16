using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;
using VendorManagementSystem.Infrastructure.Utility;
using Abp.Collections.Extensions;
using Castle.MicroKernel.Registration;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class SalesInvoiceRepository : ISalesInvoiceRepository
    {
        private readonly DataContext _db;
        public SalesInvoiceRepository(DataContext db) {
            _db = db; 

        }
        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public string GetLastId()
        {
            int id = _db.SalesInvoice.OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault();
            string identifier = Util.GenerateIdentifier("EX2INV", id, 7);
            return identifier;
        }
        public int AddSalesInvoice(SalesInvoice salesInvoice)
        {
            _db.SalesInvoice.Add(salesInvoice);
            _db.SaveChanges();
            return salesInvoice.Id;
        }
        public int UpdateSalesInvoice(int id, SalesInvoice salesInvoice)
        {
            SalesInvoice? selectedInvoice = _db.SalesInvoice.Where(i => i.Id == id).FirstOrDefault();
            if(selectedInvoice == null)
            {
                return -1;
            }
            selectedInvoice.CreatorId = salesInvoice.CreatorId;
            selectedInvoice.VendorId = salesInvoice.VendorId;
            selectedInvoice.DestinationId = salesInvoice.DestinationId;
            selectedInvoice.Reference = salesInvoice.Reference;
            selectedInvoice.Date = salesInvoice.Date;
            selectedInvoice.Status = salesInvoice.Status;
            selectedInvoice.DueDate = salesInvoice.DueDate;
            selectedInvoice.PaymentTerms = salesInvoice.PaymentTerms;
            selectedInvoice.Amount = salesInvoice.Amount;
            selectedInvoice.AmountPaid = salesInvoice.AmountPaid;
            selectedInvoice.CreatedAt = salesInvoice.CreatedAt;
            selectedInvoice.UpdatedAt = salesInvoice.UpdatedAt;
            selectedInvoice.CreatedBy = salesInvoice.CreatedBy;
            selectedInvoice.UpdatedBy = salesInvoice.UpdatedBy;
            _db.SaveChanges();
            return id;
        }
        public IEnumerable<SalesInvoiceResponseDto> GetAllSalesInvoice(PaginationDto paginationDto,string? filter)
        {
            int cursor = paginationDto.Cursor;
            int size = paginationDto.Size;
            bool next = paginationDto.Next;
            if (cursor == 0)
            {
                cursor = _db.SalesInvoice.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault() + 1;
            }
            IQueryable<SalesInvoice> query = _db.SalesInvoice;
            if(!filter.IsNullOrEmpty())
            {
                query = query.Where(p => p.Vendor!=null&&p.Vendor.CompanyName.Contains(filter ?? "") && p.VendorId != null);
            }
            if (next)
            {
                query = query
                    .OrderByDescending(p => p.Id)
                    .Where(p => p.Id < cursor && p.VendorId != null)
                    .Take(size);
            }
            else
            {
                query = query
                    .OrderBy(p => p.Id)
                    .Where(p => p.Id > cursor && p.VendorId != null)
                    .Take(size)
                    .OrderByDescending(p => p.Id);
            }
            var data = query.Select(q => new SalesInvoiceResponseDto
            {
                Id = q.Id,
                Identifier = Util.GenerateIdentifier("EX2INV", q.Id, 7),
                CreatedOn = q.Date,
                DueDate = q.DueDate,
                Reference = q.Reference??string.Empty,
                VendorName = q.Vendor!=null?q.Vendor.CompanyName:string.Empty,
                Status = q.Status.ToString(),
                Amount = q.Amount,
            }).ToList();
            return data;
        }
        public bool HasNeighbour(int cursor, bool next, string? filter)
        {
            if (cursor == 0)
            {
                cursor = _db.SalesInvoice.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault() + 1;
            }
            IQueryable<SalesInvoice> query = _db.SalesInvoice;
            if (!filter.IsNullOrEmpty())
            {
                query = query.Where(p => p.Vendor!=null&&p.Vendor.CompanyName.Contains(filter??""));
            }
            if (next)
            {
                query = query.Where(p => p.VendorId != null && p.Id < cursor);
            }
            else
            {
                query = query.Where(p => p.VendorId != null && p.Id > cursor);
            }
            return query.Any();
        }

        public bool DeleteSalesInvoice(int id)
        {
            var invoice = _db.SalesInvoice.Where(si => si.Id == id).FirstOrDefault();
            if (invoice != null)
            {
                _db.SalesInvoice.Remove(invoice);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public SalesInvoiceEmailDto? GetSlalesInvoiceById(int id)
        {
            return _db.SalesInvoice.Where(invoice => invoice.Id == id).Select(invoice => new SalesInvoiceEmailDto
            {
                BuyingVendor = invoice.VendorId ?? 0,
                SellingVendor = invoice.CreatorId ?? 0,
                InvoiceId = Util.GenerateIdentifier("EX2INV", invoice.Id, 7),
                InvoiceDate = invoice.Date ?? default(DateTime),
                Terms = invoice.PaymentTerms.ToString(),
                DueDate = invoice.DueDate,
                PlaceOfSupply = invoice.DestinationState!=null? invoice.DestinationState.Name:"Unkonw Destination",
                Items = new List<SaleItem>(),
                AmountPaid = invoice.AmountPaid,
            }).FirstOrDefault();
        }
    }
}
