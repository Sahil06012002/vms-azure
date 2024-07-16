using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DataContext _db;
        public UnitRepository(DataContext db)
        {
            _db = db;
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            return _db.Unit.ToList();
        }
    }
}
