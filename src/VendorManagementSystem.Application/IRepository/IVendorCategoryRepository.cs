using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IVendorCategoryRepository
    {
        bool AddVendorCategoryEntry(List<VendorCategoryMapping> entries);
        public bool toggleStatus(int vendorId, List<int> categoryIds, bool status);
        public int GetVendorCategoryId(int vendorId, int categoryId);
        public IEnumerable<object> GetAssociatedCategories(int vendorId);
        public List<int> GetAssociatedCategoriesIds(int vendorId);
    }
}
