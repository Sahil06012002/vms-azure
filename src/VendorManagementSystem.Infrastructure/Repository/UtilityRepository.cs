

using Microsoft.EntityFrameworkCore;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class UtilityRepository : IUtilityRespository
    {
        private readonly DataContext _db;

        public UtilityRepository(DataContext db)
        {
            _db = db;
        }
        public IEnumerable<State> GetStatesData()
        {
            return _db.States.ToList();
        }

        public IEnumerable<VendorTdsOption> GetTDSOptions()
        {
            return _db.VendorTDSOptions.ToList();
        }

        public string? GetStateNameById(int id)
        {
            return _db.States.Where(state => state.Id == id).FirstOrDefault()?.Name;
        }
    }
}
