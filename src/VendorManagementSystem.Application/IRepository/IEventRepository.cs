using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IEventRepository
    {
        public Event? AddEvent(Event even);

        public IEnumerable<Event> GetAllEvents(PaginationDto paginationDto, string? filter);

        public bool NeighbourExsistance(int id, bool next);

        // public IEnumerable<Expenditure> GetTopExpenditure(int count);
        public IEnumerable<Event> GetTopEvents(int count);
    }
}
