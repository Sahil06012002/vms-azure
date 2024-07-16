using VendorManagementSystem.Application.Dtos.ModelDtos.Invoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos;

namespace VendorManagementSystem.Application.IServices
{
    public interface IInvoiceService
    {
        public Task<ApplicationResponseDto<object>> CreateInvoice(CreateInvoiceDto createInvoiceDto,string jwtToken);
        //public ApplicationResponseDto<object> CreateInvoice(CreateInvoiceDto createInvoiceDto, string jwtToken);
        public ApplicationResponseDto<PagenationResponseDto> GetInvoices(string? filter,int cursor, int size, bool next);
        public Task<ApplicationResponseDto<FileContentDto>> GetFile(string name);
    }
}
