using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PuppeteerSharp;
using System.Security.Cryptography.Xml;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder;
using VendorManagementSystem.Application.Dtos.UtilityDtos.SalesInvoice;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Utilities;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{

    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly ISalesInvoiceRepository _salesInvoiceRepository;
        private readonly ITokenService _tokenService;
        private readonly IVendorRepository _vendorRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUtilityRespository _utilityRespository;
        private readonly ISelectedItemsRepository _selectedItemsRepository;
        private readonly IUtilityService _utilityService;
        private readonly IEmailService _emailService;
        private readonly IUtilityRespository _utilityRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IErrorLoggingService _errorLoggingService;
        private readonly IDigitalSign _digitalSign;

        public SalesInvoiceService(IDigitalSign digitalSign,ISalesInvoiceRepository salesInvoiceRepository, ITokenService tokenService, IVendorRepository vendorRepository, IAddressRepository addressRepository, IUtilityRespository utilityRespository, ISelectedItemsRepository selectedItemsRepository, IUtilityService utilityService, IEmailService emailService, IUtilityRespository utilityRepository, IItemRepository itemRepository, IErrorLoggingService errorLoggingService)
        {
            _salesInvoiceRepository = salesInvoiceRepository;
            _tokenService = tokenService;
            _vendorRepository = vendorRepository;
            _addressRepository = addressRepository;
            _utilityRespository = utilityRespository;
            _selectedItemsRepository = selectedItemsRepository;
            _utilityService = utilityService;
            _emailService = emailService;
            _utilityRepository = utilityRepository;
            _itemRepository = itemRepository;
            _errorLoggingService = errorLoggingService;
            _digitalSign = digitalSign;
        }

        public ApplicationResponseDto<object> GetInvoiceFormData(string jwtToken)
        {
            ApplicationResponseDto<object> result = new();
            try
            {
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                IEnumerable<object> vendor = _vendorRepository.VednorFormDetails();
                //add a entry first
                SalesInvoice salesInvoice = new()
                {
                    DestinationId = 3,
                    PaymentTerms = PaymentTerms.PrePaid,
                    Status = PurchaseStatus.Draft,
                    Amount = 0,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = int.Parse(currentUser),

                };
                int lastId = _salesInvoiceRepository.AddSalesInvoice(salesInvoice);
                string identifier = _utilityService.GenerateIdentifier("EX2INV", lastId, 7);
                IEnumerable<State> states = _utilityRespository.GetStatesData();
                string[] paymentTerms = Enum.GetNames(typeof(PaymentTerms));
                string[] status = Enum.GetNames(typeof(PurchaseStatus));
                IEnumerable<object> addresses = _addressRepository.RelatedAddresses([87, 88]);

                SalesInvoiceFormDto response = new()
                {
                    Id = lastId,
                    Identifier = identifier,
                    Vendors = vendor,
                    States = states,
                    Addresses = addresses,
                    PaymentTerms = paymentTerms,
                    Status = status
                };
                result.Data = response;
                return result;
            }
            catch (Exception ex)
            {
                result.Error = new() { Message = [ex.Message] };
                return result;
            }
        }

        public async Task<ApplicationResponseDto<int>> AddSalesInvoice(SalesInvoiceDto salesInvoiceDto, string jwtToken)
        {
            ApplicationResponseDto<int> result = new();
            try
            {
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                PaymentTerms paymentTerms = _utilityService.ParseEnum<PaymentTerms>(salesInvoiceDto.PaymentTerms);
                PurchaseStatus purchaseStatus = _utilityService.ParseEnum<PurchaseStatus>(salesInvoiceDto.Status);


                Console.WriteLine($"********** amount: {salesInvoiceDto.Amount} and status: {salesInvoiceDto.Status} --> {purchaseStatus}");

                SalesInvoice salesInvoice = new()
                {
                    CreatorId = salesInvoiceDto.CreatorId,
                    VendorId = salesInvoiceDto.VendorId,
                    DestinationId = salesInvoiceDto.DestinationId,
                    Reference = salesInvoiceDto.Reference,
                    Date = salesInvoiceDto.Date,
                    DueDate = salesInvoiceDto.DueDate,
                    PaymentTerms = paymentTerms,
                    Status = purchaseStatus,
                    Amount = salesInvoiceDto.Amount,
                    AmountPaid = salesInvoiceDto.AmountPaid,
                    CreatedAt = DateTime.Today,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.Today,
                    UpdatedBy = int.Parse(currentUser),
                };
               
                using (var transaction = _salesInvoiceRepository.BeginTransaction())
                {
                    try
                    {
                        int invoiceId = _salesInvoiceRepository.UpdateSalesInvoice(salesInvoiceDto.Id, salesInvoice);
                        List<SalesInvoiceItems> items = salesInvoiceDto.selectedItems.Select(incomingItem => new SalesInvoiceItems
                        {
                            SalesInvoiceId = invoiceId,
                            ItemId = incomingItem.ItemId,
                            Account = incomingItem.Account,
                            Rate = incomingItem.Rate,
                            Quantity = incomingItem.Quantity,
                            Tax = incomingItem.Tax,
                            Amount = incomingItem.Amount // can also be calculated

                        }).ToList();

                        bool status = _selectedItemsRepository.AddSelectedItems(invoiceId, items);
                        if (!status)
                        {
                            return new ApplicationResponseDto<int>
                            {
                                Error = new Error
                                {
                                    Message = ["Error while saving changes to db"]
                                }
                            };
                        }
                        if (purchaseStatus == PurchaseStatus.Issued)
                        {
                            SalesInvoiceEmailDto salesInvoiceEmailDto = new()
                            {
                                SellingVendor = salesInvoice.CreatorId ?? 87,
                                BuyingVendor = salesInvoiceDto.VendorId,
                                InvoiceDate = salesInvoiceDto.Date,
                                DueDate = salesInvoice.DueDate,
                                InvoiceId = "EX2INV" + salesInvoiceDto.Id.ToString().PadLeft(7, '0'),
                                PlaceOfSupply = _utilityRepository.GetStateNameById(salesInvoiceDto.DestinationId) ?? "",
                                Terms = salesInvoiceDto.PaymentTerms,
                                Items = salesInvoiceDto.selectedItems.Select(item => new SaleItem
                                {
                                    ItemName = item.ItemName,
                                    Quantity = item.Quantity,
                                    Amount = item.Amount,
                                    CGST = item.Tax / 2.0f,
                                    SGST = item.Tax / 2.0f,
                                    HSN = item.HSN,
                                    Rate = item.Rate,
                                }).ToList(),
                                AmountPaid = salesInvoiceDto.AmountPaid,
                                Body = salesInvoiceDto.Body??"Please find the attached Sales Invoice below",
                                Subject = salesInvoiceDto.Subject??"EX Squared Sales Invoice",
                            };
                            
                            var pdfResult = await SendSalesInvoiceMailAsync(salesInvoiceEmailDto, jwtToken);
                            if (pdfResult.Error != null)
                            {
                                return new ApplicationResponseDto<int>
                                {
                                    Error = new Error
                                    {
                                        Message = ["Error while generating and sending pdf"]
                                    }
                                };
                            }
                        }
                        transaction.Commit();
                        result.Data = invoiceId;
                        return result;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        result.Data = -1;
                        result.Error = new() { Message = [ex.Message] };
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Error = new() { Message = [ex.Message] };
                return result;
            }
        }
        public ApplicationResponseDto<PagenationResponseDto> GetSalesInvoice(PaginationDto paginationDto, string? filter)
        {
            ApplicationResponseDto<PagenationResponseDto> result = new();
            try
            {
                PagenationResponseDto response = new();
                IEnumerable<SalesInvoiceResponseDto> records= _salesInvoiceRepository.GetAllSalesInvoice(paginationDto, filter);
                if (records == null || !records.Any())
                {
                    Console.WriteLine("incoming data from is null");
                    response.PagenationData = [];
                    response.PreviousCursor = 0;
                    response.Cursor = 0;
                    response.HasNextPage = false;
                    response.HasPreviousPage = false;
                }
                else
                {
                    int prevCursor = records.First().Id;
                    int cursor = records.Last().Id;
                    bool hasNextPage = _salesInvoiceRepository.HasNeighbour(cursor, true, filter);
                    bool hasPrevPage = _salesInvoiceRepository.HasNeighbour(prevCursor, false, filter);
                    response.PagenationData = records;
                    response.PreviousCursor = prevCursor;
                    response.Cursor = cursor;
                    response.HasNextPage = hasNextPage;
                    response.HasPreviousPage = hasPrevPage;
                }
                result.Data = response;
                return result;
            }
            catch (Exception ex)
            {
                result.Error = new() { Message = [ex.Message] };
                return result;
            }
        }

        public async Task<ApplicationResponseDto<FileContentDto>> DownloadSalesInvoiceAsync(int SalesInvoiceId, string token)
        {
            try
            {
                var invoice = _salesInvoiceRepository.GetSlalesInvoiceById(SalesInvoiceId);
                if (invoice != null)
                {
                    invoice.Items = _itemRepository.GetSalesInoiveItems(SalesInvoiceId);
                    var pdfBytes = await GeneratePdf(invoice, token, null, null);
                    
                    return new ApplicationResponseDto<FileContentDto>
                    {
                        Data = new FileContentDto
                        {
                            Content = pdfBytes,
                            ContentType = "application/pdf",
                            Name = $"{invoice.InvoiceId}.pdf"
                        }
                    };
                }
                return new ApplicationResponseDto<FileContentDto>
                {
                    Message = "Invoice not Found",
                };
            }
            catch(Exception ex)
            {
                _errorLoggingService.LogError(ex.ToString());
                return new ApplicationResponseDto<FileContentDto>
                {
                    Message = "Unable to get Pdf",
                };
            }
        }

        public async Task<byte[]> GeneratePdf(SalesInvoiceEmailDto salesInvoiceEmailDto, string token, VendorNewResponseDto? vendorSelling, VendorNewResponseDto? vendorBuying)
        {
            VendorNewResponseDto? sellingVendor = vendorSelling, buyingVendor = vendorBuying;
            if (sellingVendor == null)
            {
                sellingVendor = _vendorRepository.GetVendorById(salesInvoiceEmailDto.SellingVendor);
            }
            if (buyingVendor == null)
            {
                buyingVendor = _vendorRepository.GetVendorById(salesInvoiceEmailDto.BuyingVendor);
            }

            if (sellingVendor != null && buyingVendor != null)
            {
                string pdfContent = SalesInvoiceFormatter.GetPdfContent(salesInvoiceEmailDto, sellingVendor, buyingVendor);

                Console.WriteLine("Buying Vednor {0}", buyingVendor.CompanyName);

                byte[] imageBytes, signatureBytes;
                using (var fs = new FileStream("exsq_logo.png", FileMode.Open, FileAccess.Read))
                {
                    imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int)fs.Length);
                }
                using (var fs = new FileStream("signature.png", FileMode.Open, FileAccess.Read))
                {
                    signatureBytes = new byte[fs.Length];
                    fs.Read(signatureBytes, 0, (int)fs.Length);
                }
                string imageString = Convert.ToBase64String(imageBytes);
                string signature = Convert.ToBase64String(signatureBytes);
                pdfContent = pdfContent.Replace("{{IMAGE_BASE64}}", imageString);
                pdfContent = pdfContent.Replace("{{signature}}", signature);

                /*var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync();*/

                var options = new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox" }
                };
                byte[] pdfBytes;
                using (var browser = await Puppeteer.LaunchAsync(options))
                using (var page = await browser.NewPageAsync())
                {
                    await page.SetContentAsync(pdfContent);
                    using (var pdfStream = await page.PdfStreamAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            pdfStream.CopyTo(memoryStream);
                            pdfBytes = memoryStream.ToArray();
                        }
                    }
                }
                return pdfBytes;
            }
            return new byte[0];
        }
        public async Task<ApplicationResponseDto<string>> SendSalesInvoiceMailAsync(SalesInvoiceEmailDto salesInvoiceEmailDto, string token)
        {
            Console.WriteLine("****in sending pdf parent function***** {0}", salesInvoiceEmailDto.AmountPaid);
            try
            {
                var sellingVendor = _vendorRepository.GetVendorById(salesInvoiceEmailDto.SellingVendor);
                var buyingVendor = _vendorRepository.GetVendorById(salesInvoiceEmailDto.BuyingVendor);
                var email = _tokenService.ExtractUserDetials(token, "email");
                var name = _tokenService.ExtractUserDetials(token, "username");
                if (sellingVendor != null && buyingVendor != null)
                {

                    byte[] pdfBytes = await GeneratePdf(salesInvoiceEmailDto, token, sellingVendor, buyingVendor);

                    byte[] digitalSignature = _digitalSign.SignDocument(pdfBytes);
                    await Console.Out.WriteLineAsync("received digital signature");
                    Console.WriteLine(Convert.ToBase64String(digitalSignature));


                    using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
                    {
                        PdfDocument pdf = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
                        pdf.Info.Elements["/Signature"] = new PdfString(Convert.ToBase64String(digitalSignature));
                        using (MemoryStream outPutStream = new MemoryStream())
                        {
                            pdf.Save(outPutStream);
                            pdfBytes = outPutStream.ToArray();
                        }
                    }

                     AckEmailDto ackEmailDto = new()
                    {
                        Body = salesInvoiceEmailDto.Body??"Please find the below attached Sales Invoice",
                        Subject = salesInvoiceEmailDto.Subject??$"{salesInvoiceEmailDto.InvoiceId}: EX2India Sales Invoice.",
                        FromEmailAddress = email,
                        FromName = name,
                        ToEmailAddress = buyingVendor.PrimaryContact!.Email,
                        ToName = $"{buyingVendor.PrimaryContact.FirstName} {buyingVendor.PrimaryContact.LastName}",
                        Pdf = new FileContentDto
                        {
                            Name = $"{salesInvoiceEmailDto.InvoiceId}.pdf",
                            Content = pdfBytes,
                            ContentType = "application/pdf"
                        }
                    };

                    _emailService.SendAckEmail(ackEmailDto);

                    return new ApplicationResponseDto<string>
                    {
                        Message = $"Completed",
                    };
                }
                return new ApplicationResponseDto<string>
                {
                    Message = "Not Sent",
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ApplicationResponseDto<string>
                {
                    Message = "Not Sent",
                };
            }
        }

        public ApplicationResponseDto<bool> DeleteSalesInvoice(int id)
        {
            ApplicationResponseDto<bool> result = new();
            try
            {
                var response = _salesInvoiceRepository.DeleteSalesInvoice(id);
                result.Data = response;
                if (!response)
                {
                    result.Error = new() { Message = ["invoice not found"] };
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Error = new() { Message = [ex.Message] };
                return result;
            }
        }
    }
}
