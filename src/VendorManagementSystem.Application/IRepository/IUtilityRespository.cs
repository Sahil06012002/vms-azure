using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IUtilityRespository
    {
        public IEnumerable<State> GetStatesData();
        public IEnumerable<VendorTdsOption> GetTDSOptions();
        public string? GetStateNameById(int id);
    }
}
