using Newtonsoft.Json.Linq;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly ITokenService _tokenService;
        private readonly IErrorLoggingService _errorLog;
        private readonly IVendorCategoryRepository _vendorCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUtilityRespository _utilityRespository;
        private readonly IUtilityService _utilityService;
        public VendorService(IVendorRepository vendorRepository, ITokenService tokenService, IErrorLoggingService errorLog, IVendorCategoryRepository vendorCategoryRepository, ICategoryRepository categoryRepository, IUtilityRespository utilityRespository, IUtilityService utilityService)
        {
            _vendorRepository = vendorRepository;
            _tokenService = tokenService;
            _errorLog = errorLog;
            _vendorCategoryRepository = vendorCategoryRepository;
            _categoryRepository = categoryRepository;
            _utilityRespository = utilityRespository;
            _utilityService = utilityService;
        }


        public ApplicationResponseDto<int> CreateVendor(CreateVendorNewDto vendornNewDto, string jwtToken)
        {
            try
            {
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
                

                Currency currency = (Currency)Enum.Parse(typeof(Currency), vendornNewDto.Currency);
                PaymentTerms paymentTerms = (PaymentTerms)Enum.Parse(typeof(PaymentTerms), vendornNewDto.PaymentTerms);
                Country shippingCountry = (Country)Enum.Parse(typeof(Country), vendornNewDto!.ShippingDto!.Country);
                Country billingCountry = (Country)Enum.Parse(typeof(Country), vendornNewDto!.BillingDto!.Country);
                Salutation salutation = (Salutation)Enum.Parse(typeof(Salutation), vendornNewDto.Salutation);


                VendorNew vendorNew = new()
                {
                    Status = true,
                    CompanyName = vendornNewDto.CompanyName,
                    GSTIN = vendornNewDto.GSTIN,
                    Currency = currency,
                    PaymentTerms = paymentTerms,
                    TDSId = vendornNewDto.TDSId,
                    VendorTypeId = vendornNewDto.TypeId,
                    FileName = string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = int.Parse(currentUser),
                };

                
                

                Address addressShipping = new()
                {
                    AddressType = AddressTypes.Shipping,
                    Attention = vendornNewDto.ShippingDto.Attention,
                    Country = shippingCountry,
                    AddressLine1 = vendornNewDto.ShippingDto.AddressLine1,
                    AddressLine2 = vendornNewDto.ShippingDto.AddressLine2,
                    City = vendornNewDto.ShippingDto.City,
                    StateId = vendornNewDto.ShippingDto.StateId,
                    PinCode = vendornNewDto.ShippingDto.PinCode,
                    Phone = vendornNewDto.ShippingDto.Phone,
                    FaxNumber = vendornNewDto.ShippingDto.FaxNumber,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = int.Parse(currentUser),
                };

                Address addressBilling = new()
                {
                    AddressType = AddressTypes.Billing,
                    Attention = vendornNewDto.BillingDto.Attention,
                    Country = billingCountry,
                    AddressLine1 = vendornNewDto.BillingDto.AddressLine1,
                    AddressLine2 = vendornNewDto.BillingDto.AddressLine2,
                    City = vendornNewDto.BillingDto.City,
                    StateId = vendornNewDto.BillingDto.StateId,
                    PinCode = vendornNewDto.BillingDto.PinCode,
                    Phone = vendornNewDto.BillingDto.Phone,
                    FaxNumber = vendornNewDto.BillingDto.FaxNumber,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = int.Parse(currentUser),
                };

                PrimaryContact primaryContact = new()
                {
                    Salutation = salutation,
                    FirstName = vendornNewDto.FirstName,
                    LastName = vendornNewDto.LastName,
                    Email = vendornNewDto.Email,
                    WorkPhone = vendornNewDto.WorkPhone,
                    MobilePhone = vendornNewDto.MobilePhone,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = int.Parse(currentUser),
                };

                using (var transaction = _vendorRepository.BeginTransaction())
                {
                    try
                    {
                        int vendorId = _vendorRepository.AddVendor(vendorNew);
                        addressShipping.VendorId = vendorId;
                        addressBilling.VendorId = vendorId;
                        
                        _vendorRepository.AddVendorAddress(new List<Address> { addressShipping, addressBilling });
                        primaryContact.VendorId = vendorId;
                        _vendorRepository.AddVendorPrimaryContact(primaryContact);
                        var entries = vendornNewDto.CategoryIds?.Select(categoryId => new VendorCategoryMapping
                        {
                            VendorId = vendorId,
                            CategoryId = categoryId,
                            Status = true
                        }).ToList();
                        if (entries != null)
                        {
                            _vendorCategoryRepository.AddVendorCategoryEntry(entries);
                        }
                        transaction.Commit();
                        return new ApplicationResponseDto<int>
                        {
                            Data = vendorId,
                        };


                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        return new ApplicationResponseDto<int>
                        {
                            Data = -1,
                            Error = new()
                            {
                                Code = (int)ErrorCodes.InternalError,
                                Message = new List<string> { ex.Message },
                            },
                        };
                    }
                }
            }catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<int>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }



        public ApplicationResponseDto<PagenationResponseDto> GetAllVendors(string? filter,int cursor, int size,bool next)
        {
            try
            {
                if (cursor < 0 || size <= 0)
                {
                    return new ApplicationResponseDto<PagenationResponseDto>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string>() { $"Inputs {cursor} or {size} is invalid. 'LastId should not be negative and Pagesize should be greater than 0'" }
                        }
                    };
                }
                var vendors= _vendorRepository.GetVendorsNew(filter, cursor, size, next);
                int currentLastId = 0, currentFirstId = 0;
                if (vendors.Any())
                {
                    currentLastId = vendors.ElementAt(vendors.Count() - 1).VendorId;
                    currentFirstId = vendors.ElementAt(0).VendorId;
                }

                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Data = new PagenationResponseDto
                    {
                        PagenationData = vendors,
                        Cursor = currentLastId,
                        PreviousCursor = currentFirstId,
                        HasNextPage = (currentLastId != 0) && _vendorRepository.HasNeighbour(filter ?? string.Empty, currentLastId, true),
                        HasPreviousPage = (currentFirstId != 0) && _vendorRepository.HasNeighbour(filter ?? string.Empty, currentFirstId, false),
                    }
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }
        }



        public ApplicationResponseDto<VendorNewResponseDto> GetVendorById(int vendorId)
        {
            try
            {
                VendorNewResponseDto? vendor = _vendorRepository.GetVendorById(vendorId);
                if (vendor != null)
                {
                    return new ApplicationResponseDto<VendorNewResponseDto>
                    {
                        Data = vendor
                    };
                }
                else
                {
                    return new ApplicationResponseDto<VendorNewResponseDto>
                    {
                        Data = null,
                        Message = "Vendor not found in the database"
                    };
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<VendorNewResponseDto>
                {
                    Data = null,
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { "Internal Db error" },
                    }
                };
            }
        }

        public ApplicationResponseDto<bool> UpdateVendor(UpdateVendorNewDto updateDto, string jwtToken)
        {
            string currentUser = _tokenService.ExtractUserDetials(jwtToken, "id");
            try
            {
              
               var vendorId = updateDto.Id;
                var categoryIds = updateDto.Categories;
                HashSet<int> existingCategories = new HashSet<int>(_vendorCategoryRepository.GetAssociatedCategoriesIds(vendorId));
                HashSet<int> incomingCategories = new HashSet<int>(categoryIds);

                HashSet<int> commonEntries = new HashSet<int>(existingCategories);
                commonEntries.IntersectWith(incomingCategories);
                HashSet<int> categoriesToInsert = new HashSet<int>(incomingCategories);
                categoriesToInsert.ExceptWith(existingCategories);

                HashSet<int> categoriesToDisable = new HashSet<int>(existingCategories);
                categoriesToDisable.ExceptWith(incomingCategories);

                

                using (var transaction = _vendorRepository.BeginTransaction())
                {
                    try
                    {
                        if(updateDto.VendorColumns != null && updateDto.VendorColumns.Any())
                        {
                         _vendorRepository.UpdateVendor(vendorId, updateDto.VendorColumns, currentUser);
                        }
                        if (updateDto.VendorColumns != null && updateDto.ShippingAddressColumns.Any())
                        {
                        _vendorRepository.UpdateAddress(vendorId, updateDto.ShippingAddressColumns, AddressTypes.Shipping, int.Parse(currentUser));
                        }
                        if (updateDto.VendorColumns != null && updateDto.BillingAddressColumns.Any())
                        {

                        _vendorRepository.UpdateAddress(vendorId, updateDto.BillingAddressColumns, AddressTypes.Billing, int.Parse(currentUser));
                        }
                        if(updateDto.VendorColumns != null && updateDto.PrimaryContacts.Any())
                        {
                            _vendorRepository.UpdatePrimaryContact(vendorId, updateDto.PrimaryContacts, int.Parse(currentUser));
                        }
                        var entriesToInsert = categoriesToInsert.Select(categoryId => new VendorCategoryMapping
                        {
                            VendorId = vendorId,
                            CategoryId = categoryId,
                            Status = true
                        }).ToList();

                        if (entriesToInsert.Any())
                        {
                            _vendorCategoryRepository.AddVendorCategoryEntry(entriesToInsert);
                        }
                        //to reduce redundency we can also delete the vcm if no invoices and contracts are associated with it , but this case doesnt seem reasonable
                        _vendorCategoryRepository.toggleStatus(vendorId,categoriesToDisable.ToList(),false);
                        _vendorCategoryRepository.toggleStatus(vendorId, commonEntries.ToList(),true);

                        transaction.Commit();
                        return new ApplicationResponseDto<bool>
                        {
                            Data = true,
                        };
                    }
                    catch (Exception ex)
                    {
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        transaction.Rollback();
                        return new ApplicationResponseDto<bool>
                        {
                            Data = false,
                            Error = new()
                            {
                                Code = (int)ErrorCodes.InternalError,
                                Message = new List<string> { ex.Message },
                            },
                        };
                    }
                }


            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<bool>
                {
                    Data = false,
                    Error = new()
                    {
                        Code = (int)ErrorCodes.DatabaseError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }




        }

        public ApplicationResponseDto<List<string>> GetVendorNewProperties()
        {
            try
            {
                var vendor = new CreateVendorNewDto();
                List<string> propertyNames = _utilityService.ExtractPropertyNames(vendor);
                return new ApplicationResponseDto<List<string>>
                {
                    Data = propertyNames
                };
            }catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<List<string>>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }
        public ApplicationResponseDto<bool> ToogleVendorStatus(int vendorId,string jwtToken)
        {
            
            try
            {
                string id = _tokenService.ExtractUserDetials(jwtToken, "id");
                int currentUser = int.Parse(id);
                
                var response = _vendorRepository.ToggleStatus(vendorId, currentUser);
                return new ApplicationResponseDto<bool>
                {
                    Data = response,
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<bool>
                {
                    Data = false,
                    Error = new()
                    {
                        Code = (int)ErrorCodes.DatabaseError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }
        }

        public ApplicationResponseDto<VendorFormDataDto> GetFormData()
        {
            try
            {
                IEnumerable<VendorFormTypesDto> types = _vendorRepository.GetTypesForForm();
                IEnumerable<VendorFormCategoriesDto> categories = _categoryRepository.GetCategoriesForForm();
                return new ApplicationResponseDto<VendorFormDataDto>
                {
                    Data = new VendorFormDataDto()
                    {
                        Categories = categories,
                        VednorTypes = types,
                        Salutations = new List<string>(Enum.GetNames(typeof(Salutation))),
                        Currency = new List<string>(Enum.GetNames(typeof(Currency))),
                        AddressTypes = new List<string>(Enum.GetNames(typeof(AddressTypes))),
                        Country = new List<string>(Enum.GetNames(typeof(Country))),
                        PaymentTerms = new List<string>(Enum.GetNames(typeof(PaymentTerms))),
                        States = _utilityRespository.GetStatesData(),
                        TDSOptions = _utilityRespository.GetTDSOptions(),
                    }
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<VendorFormDataDto>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }

        }
    }
}


