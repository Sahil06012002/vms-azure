using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        
            private readonly DataContext _db;

            public EventRepository(DataContext db)
            {
                _db = db;
            }
            public Event? AddEvent(Event even)
            {
                _db.Events.Add(even);
                int result = _db.SaveChanges();
                if (result > 0)
                {
                    return even;
                }
                return null;
            }

            public IEnumerable<Event> GetAllEvents(PaginationDto paginationDto, string? filter)
            {

                int cursor = paginationDto.Cursor, pageSize = paginationDto.Size;
                bool next = paginationDto.Next;

                IQueryable<Event> query;

                if (next)
                {
                    if (cursor == 0)
                    {
                        query = _db.Events.OrderByDescending(c => c.Id);
                    }
                    else
                    {
                        query = _db.Events.OrderByDescending(expenditure => expenditure.Id).Where(expenditure => expenditure.Id < cursor);
                    }
                }
                else
                {
                    query = _db.Events.OrderBy(expenditure => expenditure.Id).Where(expenditure => expenditure.Id > cursor);
                }

                Console.WriteLine("1 => ", query.Count());
                // if (next) return response;
                return query.OrderByDescending(c => c.Id);

            }

            public bool NeighbourExsistance(int id, bool next)
            {
                IQueryable<Event> query = _db.Events.OrderByDescending(c => c.Id);
                bool result = next ? query.Any(c => c.Id < id) : query.Any(c => c.Id > id);
                return result;
            }


 
        public IEnumerable<Event> GetTopEvents(int count)
        {
            IQueryable<Event> query = _db.Events.OrderByDescending(e => e.Budget).Take(count);
            return query.OrderByDescending(c => c.Id);
        }
    }
}
