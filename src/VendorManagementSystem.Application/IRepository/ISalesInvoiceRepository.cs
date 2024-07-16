using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public  interface ISalesInvoiceRepository
    {
        public IDbContextTransaction BeginTransaction();
        public string GetLastId();
        public int AddSalesInvoice(SalesInvoice salesInvoice);
        public int UpdateSalesInvoice(int id, SalesInvoice salesInvoice);
        public IEnumerable<SalesInvoiceResponseDto> GetAllSalesInvoice(PaginationDto paginationDto, string? filter);
        public bool HasNeighbour(int cursor, bool next, string? filter);
        public bool DeleteSalesInvoice(int id);
        public SalesInvoiceEmailDto? GetSlalesInvoiceById(int id);
    }
}
