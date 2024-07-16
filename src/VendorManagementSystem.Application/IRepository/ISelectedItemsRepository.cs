using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface ISelectedItemsRepository
    {
        public bool AddSelectedItems(int invoiceId, List<SalesInvoiceItems> selectedItems);
    }
}
