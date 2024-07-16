using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.Item;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Item;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public  interface IItemRepository
    {
        public int AddItem(Item item);
        public IEnumerable<ItemResponseDto> GetItems();
        public List<SaleItem> GetSalesInoiveItems(int id);
    }
}
