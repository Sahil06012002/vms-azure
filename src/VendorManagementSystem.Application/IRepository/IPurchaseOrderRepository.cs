using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IPurchaseOrderRepository
    {
        public IDbContextTransaction BeginTransaction();
        public int AddPurchaseOrder(PurchaseOrder purchaseOrder);
        public int UpdatePurchaseOrder(int id, PurchaseOrder purchaseOrder);
        //public GetPurchasedItems(int id)
        public bool DeletePurchaseOrder(int id);

        public IEnumerable<PurchaseOrderUtilityResponseDTO> GetOrders(PaginationDto paginationDto, string? filter);
        public bool HasNeighbour(int cursor, bool next, string? filter);
        public PurchaseOrder? GetPurchaseOrderById(int id);
    }
}
