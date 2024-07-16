
using Microsoft.IdentityModel.Tokens;
using PdfSharp.Pdf.Filters;
using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.ModelDtos.Expenditure;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class ExpenditureService : IExpenditureService
    {

        private readonly IExpenditureRepository _expenditureRepository;
        private readonly IErrorLoggingService _errorLog;
        private readonly ITokenService _tokenService;

        public ExpenditureService(IExpenditureRepository expenditureRepository, IErrorLoggingService errorLog, ITokenService tokenService)
        {
            _expenditureRepository = expenditureRepository;
            _errorLog = errorLog;
            _tokenService = tokenService;
        }

        public ApplicationResponseDto<bool> AddExpenditure(List<ExpenditureDTO> expenditureDto, string token, int id)
        {
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string> { $"The jwt token is null" },
                        }
                    };
                }

                var currentUser = _tokenService.ExtractUserDetials(token!, "id");
                /*Expenditure category = new Expenditure
                {
                    Name = expenditureDto.Name,
                    Amount = expenditureDto.Amount,
                    Date = expenditureDto.Date,
                    Description = expenditureDto.Description,
                    CreatedBy = Int32.Parse(currentUser),
                    UpdatedBy = Int32.Parse(currentUser),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };*/
                List<Expenditure> expenditures = expenditureDto.Select(val => new Expenditure()
                {
                    EventId = id,
                    Name = val.Name,
                    Description = val.Description,
                    Amount = val.Amount,
                    Date = val.Date,
                    CreatedBy = int.Parse(currentUser),
                    UpdatedBy = int.Parse(currentUser),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                }).ToList() ;



                var result = _expenditureRepository.AddExpenditures(expenditures);
                if (result)
                {
                    return new ApplicationResponseDto<bool>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new ApplicationResponseDto<bool>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.DatabaseError,
                            Message = new List<string> { "Error While adding entry to Database" },
                        }
                    };
                }

            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);

                return new ApplicationResponseDto<bool>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }


        public ApplicationResponseDto<List<Expenditure>> GetAllExpenditure(int id)
        {
            ApplicationResponseDto<List<Expenditure>> result = new();
            try
            {
                var response = _expenditureRepository.GetAllExpenditure(id).ToList();
                result.Data = response;
                result.Message = "No expenditures found for this event";
                return result;
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<List<Expenditure>>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }


        public ApplicationResponseDto<IEnumerable<object>> GetTopExpenditure(int count)
        {
           try{
                IEnumerable<Expenditure> expenditures = _expenditureRepository.GetTopExpenditure(count);
                return new ApplicationResponseDto<IEnumerable<object>>
                {
                    Data = expenditures
                };
           }
           catch (Exception ex)
           {
                _errorLog.LogError((int) ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<IEnumerable<object>>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message }
                    }
                };
            }
        }

    }
}
