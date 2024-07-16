using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.Events;
using VendorManagementSystem.Application.Dtos.ModelDtos.Expenditure;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IEventService
    {
        public ApplicationResponseDto<Event> AddEvent(EventDTO eventDto, string token);

        public ApplicationResponseDto<PagenationResponseDto> GetAllEvents(PaginationDto paginationDto, string? filter);

        public ApplicationResponseDto<IEnumerable<object>> GetTopEvents(int count);
    }
}
