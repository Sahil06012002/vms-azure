using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos;

namespace VendorManagementSystem.Application.IServices
{
    public interface ISalesInvoiceService
    {
        public ApplicationResponseDto<object> GetInvoiceFormData(string jwtToken );
        public Task<ApplicationResponseDto<int>> AddSalesInvoice(SalesInvoiceDto salesInvoiceDto, string jwtToken);
        public ApplicationResponseDto<PagenationResponseDto> GetSalesInvoice(PaginationDto paginationDto, string? filter);
        public ApplicationResponseDto<bool> DeleteSalesInvoice(int id);
        public Task<ApplicationResponseDto<string>> SendSalesInvoiceMailAsync(SalesInvoiceEmailDto salesInvoiceEmailDto, string token);
        public Task<ApplicationResponseDto<FileContentDto>> DownloadSalesInvoiceAsync(int SalesInvoiceId, string token);
    }
}
