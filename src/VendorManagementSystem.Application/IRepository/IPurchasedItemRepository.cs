using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IPurchasedItemRepository
    {
        public bool AddPurchasedItems(List<PurchasedItem> items);
        public List<ItemsRow> GetPurchasedItems(int id);
    }
}
