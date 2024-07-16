using Abp.Collections.Extensions;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mysqlx.Expr;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;


namespace VendorManagementSystem.Infrastructure.Repository
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository 

    {
        private readonly DataContext _db;
        public PurchaseOrderRepository(DataContext dataContext)
        {
            _db = dataContext;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction(); 
        }
        public int AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _db.PurchaseOrders.Add(purchaseOrder);
            _db.SaveChanges();
            return purchaseOrder.Id;    
        }
        public int UpdatePurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            var existingPO = _db.PurchaseOrder.Where(po=>po.Id  == id).FirstOrDefault();
            if (existingPO != null)
            {
                existingPO.CreatorId = purchaseOrder.CreatorId;
                existingPO.VendorId = purchaseOrder.VendorId;
                existingPO.CustomerId = purchaseOrder.CustomerId;
                existingPO.SourceStateId = purchaseOrder.SourceStateId;
                existingPO.DestinationStateId = purchaseOrder.DestinationStateId;
                existingPO.Reference = purchaseOrder.Reference;
                existingPO.Date = purchaseOrder.Date;
                existingPO.DeliveryDate = purchaseOrder.DeliveryDate;
                existingPO.PaymentTerms = purchaseOrder.PaymentTerms;
                existingPO.Amount = purchaseOrder.Amount;
                existingPO.PurchaseStatus = purchaseOrder.PurchaseStatus;
                existingPO.CreatedAt = purchaseOrder.CreatedAt;
                existingPO.UpdatedAt = purchaseOrder.UpdatedAt;
                existingPO.CreatedBy = purchaseOrder.CreatedBy;
                existingPO.UpdatedBy = purchaseOrder.UpdatedBy;
            }
            else
            {
                return -1;
            }
            _db.SaveChanges();
            return existingPO.Id;
        }
        public IEnumerable<PurchaseOrderUtilityResponseDTO> GetOrders(PaginationDto paginationDto, string? filter)
        {
            if(paginationDto.Cursor == 0)
            {
                paginationDto.Cursor = _db.PurchaseOrders.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault()+1;
            }
            IQueryable<PurchaseOrder> query = _db.PurchaseOrders;
            if(!filter.IsNullOrEmpty())
            {
                query = query.Where(p =>p.Vendor!= null && p.Vendor.CompanyName.Contains(filter??string.Empty));
            }
            if(paginationDto.Next)
            {
                query = query
                    .OrderByDescending(p => p.Id)
                    .Where(p => p.VendorId != null && p.Id < paginationDto.Cursor)
                    .Take(paginationDto.Size);
            }
            else
            {
                query = query
                    .OrderBy(p => p.Id)
                    .Where(p => p.VendorId != null && p.Id > paginationDto.Cursor)
                    .Take(paginationDto.Size)
                    .OrderByDescending(p => p.Id);
            }
            var data = query.Select(q => new PurchaseOrderUtilityResponseDTO {
                                            Id = q.Id,
                                            CreatedDate = q.UpdatedAt,
                                            Identifier = "EX2PO"+ q.Id.ToString().PadLeft(9,'0'),
                                            Reference = q.Reference,
                                            VendorName = q.Vendor!=null?q.Vendor.CompanyName:string.Empty,
                                            Status = q.PurchaseStatus.ToString(),
                                            Amount = q.Amount,
                                            ExpectedDileveryDate = q.DeliveryDate 
                                            }).ToList();
            return data;
            
        }

        public bool DeletePurchaseOrder(int id)
        {
            var po = _db.PurchaseOrder.Where(po=> po.Id == id).FirstOrDefault();
            if (po != null)
            {
                _db.PurchaseOrder.Remove(po);
                _db.SaveChanges();
                return true;
            }
            return false;
            
        }

        public bool HasNeighbour(int cursor, bool next,string? filter)
        {
            if (cursor == 0)
            {
                return false;
            }
            IQueryable<PurchaseOrder> query = _db.PurchaseOrders;
            if (!filter.IsNullOrEmpty())
            {
                query = query.Where(p => p.Vendor!=null && p.Vendor.CompanyName.Contains(filter??string.Empty));
            }
            if(next)
            {
                query = query.Where(p => p.Id < cursor);
            }
            else
            {
                query = query.Where(p => p.Id > cursor);
            }
            return query.Any();
        }

        public PurchaseOrder? GetPurchaseOrderById(int id)
        {
            return _db.PurchaseOrders.Where(po => po.Id == id).FirstOrDefault();
        }
    }
}
