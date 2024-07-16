using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class SelectedItemsRepository : ISelectedItemsRepository

    {
        private readonly DataContext _db;
        public SelectedItemsRepository(DataContext db)
        {
            _db = db;
        }
        public bool AddSelectedItems(int invoiceId, List<SalesInvoiceItems> salesInvoiceItems)
        {
            _db.SalesInvoiceItems.AddRange(salesInvoiceItems);
            return _db.SaveChanges() > 0;
        }
    }
}
