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
    public class ExpenditureRepository : IExpenditureRepository
    {
        private readonly DataContext _db;

        public ExpenditureRepository(DataContext db)
        {
            _db = db;
        }
        public bool AddExpenditures(List<Expenditure> expenditure)
        {
            _db.Expenditure.AddRange(expenditure);
            int result  = _db.SaveChanges();
            return result > 0;
        }

        public IEnumerable<Expenditure> GetAllExpenditure(int id)
        {
            IEnumerable<Expenditure>  expenditures = _db.Expenditure.Where(e => e.EventId == id).ToList();
            return expenditures;

        }

        public IEnumerable<Expenditure> GetTopExpenditure(int count)
        {
            IQueryable<Expenditure> query = _db.Expenditure.OrderByDescending(e => e.Amount).Take(count);
            return query.OrderByDescending(c => c.Id);
        }
    }
}
