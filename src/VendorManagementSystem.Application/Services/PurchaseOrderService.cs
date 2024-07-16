using PuppeteerSharp;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.PurchaseOrder;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Utilities;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;
namespace VendorManagementSystem.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly ITokenService _tokenService;
        private readonly IUtilityRespository _utilityRespository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUtilityService _utilityService;
        private readonly IPurchasedItemRepository _purchasedItemRepository;
        private readonly IEmailService _emailService;
        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, IVendorRepository vendorRepository, ITokenService tokenService, IUtilityRespository utilityRepository, IAddressRepository addressRepository, IUtilityService utilityService, IPurchasedItemRepository purchasedItemRepository, IEmailService emailService)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _vendorRepository = vendorRepository;
            _tokenService = tokenService;
            _utilityRespository = utilityRepository;
            _addressRepository = addressRepository;
            _utilityService = utilityService;
            _purchasedItemRepository = purchasedItemRepository;
            _emailService = emailService;
        }


        public ApplicationResponseDto<PurchaseOrderFormDto> GetPurchaseOrderFormDetails(string jwtToken)
        {
            try
            {
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                IEnumerable<object> vendor = _vendorRepository.VednorFormDetails();
                Console.WriteLine("got vendor details");

                PurchaseOrder purchaseOrder = new()
                {
                    PaymentTerms = PaymentTerms.PrePaid,
                    Amount = 0,
                    PurchaseStatus = PurchaseStatus.Draft,
                    CreatedAt = DateTime.Today,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.Today,
                    UpdatedBy = int.Parse(currentUser),
                };

                var purchaseOrderId = _purchaseOrderRepository.AddPurchaseOrder(purchaseOrder);
                string identifier = _utilityService.GenerateIdentifier("EX2PO", purchaseOrderId, 9);

                IEnumerable<State> states = _utilityRespository.GetStatesData();
                string[] paymentTerms = Enum.GetNames(typeof(PaymentTerms));
                IEnumerable<object> addresses = _addressRepository.RelatedAddresses([87, 88]);

                PurchaseOrderFormDto response = new()
                {
                    Id = purchaseOrderId,
                    Vendor = vendor,
                    Identifier = identifier,
                    States = states,
                    PaymentTerms = paymentTerms,
                    addresses = addresses

                };


                return new ApplicationResponseDto<PurchaseOrderFormDto>
                {
                    Data = response,
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponseDto<PurchaseOrderFormDto>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.DatabaseError,
                        Message = new List<string> { ex.Message }

                    }
                };
            }
        }
        public ApplicationResponseDto<bool> AddPurchaseOrder(PurchaseOrderDto purchaseOrderDto, string jwtToken)
        {
            using (var transaction = _purchaseOrderRepository.BeginTransaction())
            {
                try
                {
                    PaymentTerms paymentTerms = _utilityService.ParseEnum<PaymentTerms>(purchaseOrderDto.PaymentTerms);
                    PurchaseStatus purchaseStatus = _utilityService.ParseEnum<PurchaseStatus>(purchaseOrderDto.PurchaseStatus);
                    string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                    PurchaseOrder purchaseOrder = new()
                    {
                        CreatorId = purchaseOrderDto.CreatorId,
                        VendorId = purchaseOrderDto.VendorId,
                        CustomerId = purchaseOrderDto.CustomerId,
                        SourceStateId = purchaseOrderDto.SourceStateId,
                        DestinationStateId = purchaseOrderDto.DestinationStateId,
                        Reference = purchaseOrderDto.Reference,
                        Date = purchaseOrderDto.Date,
                        DeliveryDate = purchaseOrderDto.DeliveryDate,
                        PaymentTerms = paymentTerms,
                        Amount = purchaseOrderDto.Amount,
                        PurchaseStatus = purchaseStatus,
                        CreatedAt = DateTime.Today,
                        CreatedBy = int.Parse(currentUser),
                        UpdatedAt = DateTime.Today,
                        UpdatedBy = int.Parse(currentUser),
                    };
                    var purchaseOrderId = _purchaseOrderRepository.UpdatePurchaseOrder(purchaseOrderDto.Id, purchaseOrder);
                    List<PurchasedItem> items = purchaseOrderDto.Items.Select(incomingItem => new PurchasedItem
                    {
                        PurchaseOrderId = purchaseOrderId,
                        ItemId = incomingItem.ItemId,
                        Account = incomingItem.Account,
                        Rate = incomingItem.Rate,
                        Quantity = incomingItem.Quantity,
                        Tax = incomingItem.Tax,
                        Amount = incomingItem.Amount
                    }).ToList();
                    var response = _purchasedItemRepository.AddPurchasedItems(items);
                    transaction.Commit();
                    return new ApplicationResponseDto<bool>
                    {
                        Data = response
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new()
                        {
                            Message = [ex.Message]
                        }
                    };
                }
            }
        }
        public ApplicationResponseDto<PagenationResponseDto> GetPurchaseOrder(PaginationDto paginationDto,string? filter)
        {
            try
            {
                IEnumerable<PurchaseOrderUtilityResponseDTO> records = _purchaseOrderRepository.GetOrders(paginationDto, filter);
                PagenationResponseDto response = new();

                    int prevCursor = records.First()?.Id ?? 0 ;
                    int cursor = records.Last()?.Id ?? 0;
                    bool hasNextPage = _purchaseOrderRepository.HasNeighbour(cursor, true, filter);
                    bool hasPrevPage = _purchaseOrderRepository.HasNeighbour(prevCursor, false, filter);
                    response.PagenationData = records;
                    response.PreviousCursor = prevCursor;
                    response.Cursor = cursor;
                    response.HasNextPage = hasNextPage;
                    response.HasPreviousPage = hasPrevPage;
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Data = response,
                };
            }catch(Exception ex)
            {
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Error = new Error
                    {
                        Message = [ex.Message]
                    }
                };
            }      

        }

        public ApplicationResponseDto<bool> DeletePurchaseOrder(int id)
        {
            try
            {
                var response = _purchaseOrderRepository.DeletePurchaseOrder(id);
                return new ApplicationResponseDto<bool>
                {
                    Data = response,
                };
            }catch (Exception ex)
            {
                return new ApplicationResponseDto<bool>
                {
                    Error = new Error
                    {
                        Message = [ex.Message],
                    }

                };
            }
        }
        public async Task<ApplicationResponseDto<FileContentDto>> DownloadPurchaseOrder(int id, string jwtToken)
        {
            try
            {
                var purchaseOrder = _purchaseOrderRepository.GetPurchaseOrderById(id);
                if (purchaseOrder != null)
                {
                    PurchaseEmailDto poEmailDto = new PurchaseEmailDto
                    {
                        DelivaryFrom = (int)purchaseOrder.VendorId!,
                        DelivaryTo = (int)purchaseOrder.CustomerId!,
                        PdfGenerationDto = new PdfGenerationDto
                        {
                            CreatorId = (int)purchaseOrder.CreatorId!,
                            DelivaryFrom = (int)purchaseOrder.VendorId!,
                            DelivaryTo = (int)purchaseOrder.CustomerId!,
                            GST = 18,
                            Date = DateOnly.FromDateTime((DateTime)purchaseOrder.Date!),
                            PurchaseOrderId = "EX2IN" + purchaseOrder.Id.ToString().PadLeft(7, '0'),
                            SubTotal = purchaseOrder.Amount,
                            Rows = _purchasedItemRepository.GetPurchasedItems(id),
                        }
                    };
                    return await GeneratePDFHTML(poEmailDto, jwtToken);
                }
                return new ApplicationResponseDto<FileContentDto>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.NotFound,
                        Message = ["Purchased order with given Id not found"],
                    }
                };
            }
            catch(Exception ex)
            {
                return new ApplicationResponseDto<FileContentDto>
                {
                    Message = "error occured",
                    Error = new()
                    {
                        Message = [ex.Message]
                    }
                };
            }
        }
        public async Task<ApplicationResponseDto<FileContentDto>> GeneratePDFHTML(PurchaseEmailDto purchaseEmail, string jwtToken, bool sendEmail=false)
        {
            try
            {
                if (purchaseEmail.PdfGenerationDto == null)
                {
                    throw new ArgumentNullException(nameof(purchaseEmail));
                }
                PdfGenerationDto generationDto = purchaseEmail.PdfGenerationDto;
                Console.WriteLine(purchaseEmail.DelivaryFrom);
                var poCreator = _vendorRepository.GetVendorById(generationDto.CreatorId);
                var customer = _vendorRepository.GetVendorById(generationDto.DelivaryTo);
                var fromVendor = _vendorRepository.GetVendorById(purchaseEmail.DelivaryFrom);

                string currentUserName = _tokenService.ExtractUserDetials(jwtToken, "username");
                string currentUserEmail = _tokenService.ExtractUserDetials(jwtToken, "email");
                if(poCreator == null || customer == null || fromVendor == null) 
                {
                    string idString=string.Empty;
                    if (poCreator == null) idString += generationDto.CreatorId;
                    if (customer == null) idString += " " + generationDto.DelivaryTo;
                    if (fromVendor == null) idString += " " + generationDto.DelivaryFrom;
                    throw new ArgumentException($"No Vendor found with {idString}");
                }
                

                string htmlContent = PurchaseOrderFormatter.getPdfContent(generationDto, fromVendor, poCreator, customer);//add cretorId , vendorId and customerId

                byte[] imageBytes;
                using (var fs = new FileStream("exsq_logo.png", FileMode.Open, FileAccess.Read))
                {
                    imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int)fs.Length);
                }

                string imageString = Convert.ToBase64String(imageBytes);

                htmlContent = htmlContent.Replace("{{IMAGE_BASE64}}", imageString);

                // var browserFetcher = new BrowserFetcher();
                // await browserFetcher.DownloadAsync();

                var options = new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox" }
                };
                byte[] pdfBytes;
                using (var browser = await Puppeteer.LaunchAsync(options))

                using (var page = await browser.NewPageAsync())
                {
                    await page.SetContentAsync(htmlContent);
                    using (var pdfStream = await page.PdfStreamAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            pdfStream.CopyTo(memoryStream);
                            pdfBytes = memoryStream.ToArray();

                        }
                    }
                }
                var fileName = $"{generationDto.PurchaseOrderId}.pdf";
                if (sendEmail)
                {
                    string toEmailAddress = "superadmin@ex2india.com", toName = "EX2India SuperAdmin";
                    if (fromVendor.PrimaryContact != null)
                    {
                        toEmailAddress = fromVendor.PrimaryContact.Email;
                        toName = $"{fromVendor.PrimaryContact.FirstName} {fromVendor.PrimaryContact.LastName}";
                    }

                    AckEmailDto poResponse = new AckEmailDto
                    {
                        ToEmailAddress = toEmailAddress,
                        ToName = toName,
                        FromEmailAddress = currentUserEmail,
                        FromName = currentUserName,
                        Body = purchaseEmail.EmailBody,
                        Subject = purchaseEmail.EmailSubject,
                        Pdf = new FileContentDto
                        {
                            Name = fileName,
                            Content = pdfBytes,
                            ContentType = "application/pdf"
                        },
                    };

                    _emailService.SendAckEmail(poResponse);
                }

                return new ApplicationResponseDto<FileContentDto>
                {
                    Data = new FileContentDto 
                    { 
                        Content = pdfBytes,
                        ContentType = "application/pdf",
                        Name = $"{generationDto.PurchaseOrderId}.pdf"
                    }
                };
            }catch(Exception ex)
            {
                return new ApplicationResponseDto<FileContentDto>
                {
                    Message = "error occured",
                    Error = new()
                    {
                        Message = [ex.Message]
                    }
                };
            }
        }
    }
}


        
