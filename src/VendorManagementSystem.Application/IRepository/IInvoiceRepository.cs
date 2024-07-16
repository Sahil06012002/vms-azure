using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IInvoiceRepository
    {
        IDbContextTransaction BeginTransaction();
        int AddInvoice(Invoice invoice);
        List<InvoiceResponseDto> GetInvoices(string? filter, int cursor, int size, bool next);
        public CountDto GetInvoiceCount();
        public bool neighbourExistance(string filter,int id, bool next);
        public ExpenditureDto GetTopCategories(int count);
    }
}
