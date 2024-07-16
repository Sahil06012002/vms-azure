using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder;

namespace VendorManagementSystem.Application.IServices
{
    public  interface IPurchaseOrderService
    {
        public ApplicationResponseDto<PurchaseOrderFormDto> GetPurchaseOrderFormDetails(string jwtToken);
        public ApplicationResponseDto<bool> AddPurchaseOrder(PurchaseOrderDto purchaseOrderDto, string jwtToken);
        public ApplicationResponseDto<PagenationResponseDto> GetPurchaseOrder(PaginationDto paginationDto, string? filter);
        public ApplicationResponseDto<bool> DeletePurchaseOrder(int id);
        public Task<ApplicationResponseDto<FileContentDto>> GeneratePDFHTML(PurchaseEmailDto purchaseEmail, string jwtToken, bool sendEmail = false);
        public Task<ApplicationResponseDto<FileContentDto>> DownloadPurchaseOrder(int id, string jwtToken);

    }
}
