using Microsoft.AspNetCore.Localization;
using System.ComponentModel;
using VendorManagementSystem.Application.Dtos.ModelDtos.Contract;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Contract;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class ContractService : IContractService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IContractRepository _contractRepository;
        private readonly IVendorCategoryRepository _vendorCategoryRepository;
        private readonly ITokenService _tokenService;
        private readonly IErrorLoggingService _errorLog;
        private readonly IVendorRepository _vendorRepository;

        public ContractService(IFileStorageService fileStorageService, IContractRepository contractRepository, IVendorCategoryRepository vendorCategoryRepository, ITokenService tokenService, IErrorLoggingService errorLog, IVendorRepository vendorRepository)
        {
            _fileStorageService = fileStorageService;
            _contractRepository = contractRepository;
            _vendorCategoryRepository = vendorCategoryRepository;
            _tokenService = tokenService;
            _errorLog = errorLog;
            _vendorRepository = vendorRepository;
        }
        public async Task<ApplicationResponseDto<Contract>> AddContract(ContractDto contractDto, string token)
        {
            try
            {
                if(contractDto.Amount<0)
                {
                    throw new InvalidDataException($"{nameof(contractDto.Amount)} can't be Negative");
                }

                if(contractDto.StartDate>contractDto.EndDate || contractDto.Amount<0)
                {
                    throw new InvalidDataException($"{nameof(contractDto.StartDate)} can't be greater than {nameof(contractDto.EndDate)}");
                }

                string currentUser = _tokenService.ExtractUserDetials(token, "id");
                // passing token to extract the user who is performing this insertion

                /*int vendortId = contractDto.VendorId;
                int contractId = contractDto.CategoryId;
                int vendorCategoryId = _vendorCategoryRepository.GetVendorCategoryId(vendortId, contractId);*/
                Contract contract = new Contract
                {
                    VendorCategoryMappingId = contractDto.VendorCategoryId,
                    Amount = contractDto.Amount,
                    ContactPersonName = contractDto.ContactPersonName,
                    ContactPersonEmail = contractDto.ContactPersonEmail,
                    ContractStatusId = contractDto.Status,
                    PaymentMode = contractDto.PaymentMode,
                    StartDate = contractDto.StartDate,
                    EndDate = contractDto.EndDate,
                    CreatedBy = int.Parse(currentUser),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedBy = int.Parse(currentUser),
                    UpdatedAt = DateTime.UtcNow,
                };
                
                
                // if file is not uploaded
                if (contractDto.File == null || contractDto.File.Length == 0)
                {
                    contract.FileName = string.Empty;
                    int res = _contractRepository.AddContract(contract);
                    if (res == 0)
                    {
                        return new ApplicationResponseDto<Contract>
                        {
                            Error = new Error
                            {
                                Code = (int)ErrorCodes.DatabaseError,
                                Message = new List<string> { "Error While adding entry to Database" },
                            }
                        };
                    }
                    return new ApplicationResponseDto<Contract>
                    {
                        Data = contract,
                        Message = "Contract added Successfully"
                    };
                }

                // if file is uploaded
                using (var transaction = _contractRepository.BeginTransaction())
                {
                    try
                    {
                        Guid id = Guid.NewGuid();
                        string fileName = $"{id}_{contractDto.File.FileName.Replace(" ", "_")}";
                        contract.FileName = fileName;
                        int res = _contractRepository.AddContract(contract);
                        if(res == 0)
                        {
                            transaction.Rollback();
                            return new ApplicationResponseDto<Contract>
                            {
                                Error = new Error
                                {
                                    Code = (int)ErrorCodes.DatabaseError,
                                    Message = new List<string> { "Error While adding entry to Database" },
                                }
                            };
                        }
                        var container = await _fileStorageService.CreateContainerIfNotExist("contracts");
                        await _fileStorageService.UploadFile(contractDto.File, fileName,container);
                        transaction.Commit();
                        if (res == 0)
                        {
                            return new ApplicationResponseDto<Contract>
                            {
                                Error = new Error
                                {
                                    Code = (int)ErrorCodes.DatabaseError,
                                    Message = new List<string> { "Error While adding entry to Database" },
                                }
                            };
                        }
                        return new ApplicationResponseDto<Contract>
                        {
                            Data = contract,
                            Message = "Contract added Successfully"
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        return new ApplicationResponseDto<Contract>
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
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<Contract>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDto<ContractFormDto> GetContractFormCreationData()
        {
            try
            {
                var vendors = _vendorRepository.VednorFormDetails();
                var contractStatuses = _contractRepository.GetContractStatus();
                return new ApplicationResponseDto<ContractFormDto>
                {
                    Data = new ContractFormDto
                    {
                        ContractStatus = contractStatuses,
                        Vendor = vendors,
                    },
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<ContractFormDto>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDto<IEnumerable<object>> GetVendorCategories(int id)
        {
            try
            {
                var response = _vendorCategoryRepository.GetAssociatedCategories(id);
                return new ApplicationResponseDto<IEnumerable<object>>
                {
                    Data = response,
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<IEnumerable<object>>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public async Task<ApplicationResponseDto<FileContentDto>> GetFile(string name)
        {
            try
            {
                var container = await _fileStorageService.CreateContainerIfNotExist("contracts");
                var response = await _fileStorageService.GetFile(name, container);
                return new ApplicationResponseDto<FileContentDto>
                {
                    Data = response,
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
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

        public ApplicationResponseDto<PagenationResponseDto> GetAllContracts(PaginationDto paginationDto, string? filter)
        {
            try
            {
                if(paginationDto.Cursor < 0 || paginationDto.Size<=0)
                {
                    return new ApplicationResponseDto<PagenationResponseDto>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string>() { $"Inputs {paginationDto.Cursor} or {paginationDto.Size} is invalid. 'LastId should not be negative and Pagesize should be greater than 0'" }
                        }
                    };
                }
                IEnumerable<ContractResponseDto> contracts = _contractRepository.GetContracts(paginationDto, filter);
                int currentLastId=0, currentFirstId=0;
                if(contracts.Any())
                {
                    currentLastId = contracts.ElementAt(contracts.Count() - 1).Id;
                    currentFirstId = contracts.ElementAt(0).Id;
                }
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Data = new PagenationResponseDto
                    {
                        PagenationData = contracts,
                        Cursor = currentLastId,
                        PreviousCursor = currentFirstId,
                        /*Size = contracts.Count(),*/
                        HasNextPage = (currentLastId != 0) && _contractRepository.NeighbourExsistance(currentLastId, true),
                        HasPreviousPage = (currentFirstId != 0) && _contractRepository.NeighbourExsistance(currentFirstId, false),
                    }
                };
            }
            catch (ArgumentOutOfRangeException)
            {
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Data = new PagenationResponseDto
                    {
                        PagenationData = [],
                        Cursor = 0,
                        PreviousCursor = 0,
                    },
                    Message = "Argument (lastId) is out of range."
                };

            }
            catch(Exception ex)
            {
                
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<PagenationResponseDto>
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
