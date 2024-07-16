using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class PurchasedItemRepository : IPurchasedItemRepository
    {
        private readonly DataContext _db;
        public PurchasedItemRepository(DataContext dataContext)
        {
            _db = dataContext;
        }
        public bool AddPurchasedItems(List<PurchasedItem> items)
        {
            _db.PurchasedItem.AddRange(items);
            int change = _db.SaveChanges();
            return change > 0;
        }

        public List<ItemsRow> GetPurchasedItems(int id)
        {
            return _db.PurchasedItem.Where(pi => pi.PurchaseOrderId == id).Select(item => new ItemsRow
            {
                Amount = item.Amount,
                ItemAndDescription = item.Item != null ? item.Item.Name : string.Empty,
                Quantity = item.Quantity,
                Rate = item.Rate,
                Tax = item.Tax,
            }).ToList();
        }
    }
}
