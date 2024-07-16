using System.Collections.Generic;
using VendorManagementSystem.Application.Dtos.ModelDtos.Contract;
using VendorManagementSystem.Application.Dtos.ModelDtos.Invoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class InvoiceService : IInvoiceService
    {

        private readonly IFileStorageService _fileStorageService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ITokenService _tokenService;
        public InvoiceService(IFileStorageService fileStorageService, IInvoiceRepository invoiceRepository, ITokenService tokenService)
        {
            _fileStorageService = fileStorageService;
            _invoiceRepository = invoiceRepository;
            _tokenService = tokenService;
        }



        //public ApplicationResponseDto<object> CreateInvoice(CreateInvoiceDto createInvoiceDto, string jwtToken)
        public async Task<ApplicationResponseDto<object>> CreateInvoice(CreateInvoiceDto createInvoiceDto, string jwtToken)
        {
            try
            {
                Console.WriteLine("---------------------------" +
                    "***********************");
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                string fileName = string.Empty;
                if (createInvoiceDto.file != null)
                {
                    Guid id = Guid.NewGuid();
                    fileName = $"{id}_{createInvoiceDto.file.FileName.Replace(" ", "_")}";
                }
                Invoice invoice = new Invoice
                {
                    VendorCategoryMappingId = createInvoiceDto.VendorCategoryMappingId,
                    ContactPersonName = createInvoiceDto.ContactPersonName,
                    ContactPersonEmail = createInvoiceDto.ContactPersonEmail,
                    Amount = createInvoiceDto.Amount,
                    DueDate = createInvoiceDto.DueDate,
                    Status = createInvoiceDto.Status,
                    FileName = fileName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Int32.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = Int32.Parse(currentUser)
                };

                Console.WriteLine(invoice.DueDate);
                
                using (var trasanction = _invoiceRepository.BeginTransaction())
                {
                    int response = _invoiceRepository.AddInvoice(invoice);
                    Console.WriteLine("inserted data");


                    if(createInvoiceDto.file != null)
                    {
                        try
                        {
                            var container = await _fileStorageService.CreateContainerIfNotExist("invoices");
                            await _fileStorageService.UploadFile(createInvoiceDto.file, fileName, container);
                            trasanction.Commit();
                            return new ApplicationResponseDto<object>()
                            {
                                Data = response,
                                Message = "Success"
                            };
                        }
                        catch (Exception ex)
                        {
                            trasanction.Rollback();
                            return new ApplicationResponseDto<object>
                            {
                                Error = new()
                                {
                                    Code = (int)ErrorCodes.AzureError,
                                    Message = new List<string> { ex.Message }
                                },
                                Message = "Error while bolb insertion"
                            };
                        }
                    }
                    trasanction.Commit();
                    return new ApplicationResponseDto<object>()
                    {
                        Data = response,
                        Message = "Success"
                    };

                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponseDto<object>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message }
                    },
                    Message = "Error while db insertion"

                };

            }
        } 
        
        public ApplicationResponseDto<PagenationResponseDto> GetInvoices(string? filter, int cursor, int size, bool next)
        {

            List<InvoiceResponseDto> invoices= _invoiceRepository.GetInvoices(filter, cursor, size,next);
            PagenationResponseDto paginatedData = new();

            if(invoices == null || !invoices.Any())
            {
                
                    paginatedData.PagenationData = [];
                    paginatedData.Cursor = 0;
                    paginatedData.PreviousCursor = 0;
                    paginatedData.HasNextPage = false;
                    paginatedData.HasPreviousPage = false;
                
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Data = paginatedData,
                    Message = "no invoices found"
                 };
            }
            int Cursor = invoices[^1].Id;
            int PreviousCursor = invoices[0].Id;
            bool nextPage = _invoiceRepository.neighbourExistance(filter??string.Empty,Cursor, true);
            bool prevPage = _invoiceRepository.neighbourExistance(filter??string.Empty,PreviousCursor, false);

            paginatedData.PagenationData = invoices;
            paginatedData.Cursor = Cursor;
            paginatedData.PreviousCursor = PreviousCursor;
            paginatedData.HasNextPage = nextPage;
            paginatedData.HasPreviousPage = prevPage;

            return new ApplicationResponseDto<PagenationResponseDto>
            {
                Data = paginatedData
            };
        }
        public async Task<ApplicationResponseDto<FileContentDto>> GetFile(string name)
        {
            try
            {
                var container = await _fileStorageService.CreateContainerIfNotExist("invoices");
                var response = await _fileStorageService.GetFile(name, container);
                return new ApplicationResponseDto<FileContentDto>
                {
                    Data = response,
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponseDto<FileContentDto>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }
    }
}
