using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.Events;
using VendorManagementSystem.Application.Dtos.ModelDtos.Expenditure;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IErrorLoggingService _errorLog;
        private readonly ITokenService _tokenService;


        public EventService(IEventRepository eventRepository, IErrorLoggingService errorLog, ITokenService tokenService)
        {
            _eventRepository = eventRepository;
            _errorLog = errorLog;
            _tokenService = tokenService;
        }


        public ApplicationResponseDto<Event> AddEvent(EventDTO expenditureDto, string token)
        {
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return new ApplicationResponseDto<Event>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string> { $"The jwt token is null" },
                        }
                    };
                }

                var currentUser = _tokenService.ExtractUserDetials(token!, "id");
                Event category = new Event
                {
                    Name = expenditureDto.Name,
                    Budget = expenditureDto.Budget,
                    Date = expenditureDto.Date,
                    Description = expenditureDto.Description,
                    Link = expenditureDto.Link,     
                    CreatedBy = Int32.Parse(currentUser),
                    UpdatedBy = Int32.Parse(currentUser),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var result = _eventRepository.AddEvent(category);
                if (result != null)
                {
                    return new ApplicationResponseDto<Event>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new ApplicationResponseDto<Event>
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

                return new ApplicationResponseDto<Event>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }


        public ApplicationResponseDto<PagenationResponseDto> GetAllEvents(PaginationDto paginationDto, string? filter)
        {
            try
            {
                Console.WriteLine("Hello");

                if (paginationDto.Cursor < 0 || paginationDto.Size <= 0)
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

                IEnumerable<Event> expenditures = _eventRepository.GetAllEvents(paginationDto, filter);

                int currentLastId = expenditures.Any() ? expenditures.ElementAt(expenditures.Count() - 1).Id : 0;
                int currentFirstId = expenditures.Any() ? expenditures.ElementAt(0).Id : 0;

                var result = expenditures.Select(expenditure => new
                {
                    expenditure.Id,
                    expenditure.Name,
                    expenditure.Description,
                    expenditure.Date,
                    expenditure.Budget,
                    expenditure.Link

                });

                Console.WriteLine("2 => ", result);
                return new ApplicationResponseDto<PagenationResponseDto>()
                {
                    Data = new PagenationResponseDto
                    {
                        PagenationData = result,
                        Cursor = currentLastId,
                        PreviousCursor = currentFirstId,
                        HasNextPage = (currentLastId != 0) && _eventRepository.NeighbourExsistance(currentLastId, true),
                        HasPreviousPage = (currentFirstId != 0) && _eventRepository.NeighbourExsistance(currentFirstId, false),
                    }
                };


            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }


        public ApplicationResponseDto<IEnumerable<object>> GetTopEvents(int count)
        {
            try
            {
                IEnumerable<Event> expenditures = _eventRepository.GetTopEvents(count);
                return new ApplicationResponseDto<IEnumerable<object>>
                {
                    Data = expenditures
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
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
