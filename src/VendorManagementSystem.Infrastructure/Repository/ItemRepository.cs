using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.Item;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Item;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _db;
        public ItemRepository(DataContext dataContext)
        {
            _db = dataContext;
        }

        public int AddItem(Item item)
        {
            _db.Item.Add(item);
            _db.SaveChanges();
            return item.Id;
        }
        public IEnumerable<ItemResponseDto> GetItems()
        {
            IEnumerable<ItemResponseDto>  items = _db.Item.Select(i => 
            new ItemResponseDto 
            { 
                Id = i.Id, 
                Name = i.Name, 
                SellingPrice = i.SellingPrice, 
                CostPrice = i.CostPrice, 
                Account = i.PurchaseAccount, 
                Gst = i.GstRate, 
                IGst = i.IGstRate,
                HSN = i.Code??"",
            }).ToList();
            return items;
        }

        public List<SaleItem> GetSalesInoiveItems(int id)
        {
            return _db.SalesInvoiceItems.Where(item => item.SalesInvoiceId == id).Select(item => new SaleItem
            {
                ItemName = item.Item!=null?item.Item.Name:"Unknown Item",
                Quantity = item.Quantity,
                CGST = item.Tax / 2.0f,
                SGST = item.Tax / 2.0f,
                HSN = item.Item!=null&&item.Item.Code!=null?item.Item.Code:string.Empty,
                Rate = item.Rate,
                Amount = item.Amount,
            }).ToList();
        }
    }
}
