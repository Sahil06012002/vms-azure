using Microsoft.EntityFrameworkCore;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;


namespace VendorManagementSystem.Infrastructure.Repository
{
    public class VendorCategoryRepository : IVendorCategoryRepository
    {
        private readonly DataContext _db;

        public VendorCategoryRepository(DataContext db)
        {
            _db = db;
        }

        public bool AddVendorCategoryEntry(List<VendorCategoryMapping> entries)
        {
            _db.VendorCategoryMappings.AddRange(entries);
            int changes = _db.SaveChanges(); 
            return changes > 0;
        }

        public int GetVendorCategoryId(int vendorId, int categoryId)
        {
            var response = _db.VendorCategoryMappings
                               .Select(vmc => new { vmc.Id, vmc.VendorId, vmc.CategoryId })
                               .Where(vcm => vcm.VendorId == vendorId && vcm.CategoryId == categoryId)
                               .FirstOrDefault();
            if (response == null)
            {
                return 0;
            }
            return response.Id;
        }
        public List<int> GetAssociatedCategoriesIds(int vendorId) {
            List<int> response = _db.VendorCategoryMappings.Where(vcm => vcm.VendorId == vendorId && vcm.CategoryId != null).Select(vcm => vcm.CategoryId ?? 0).ToList();
            return response;
        }

        public IEnumerable<object> GetAssociatedCategories(int vendorId)
        {
            var response = _db.VendorCategoryMappings.Where(vcm => vcm.VendorId == vendorId && vcm.Status).Include(vcm => vcm.Category).Select(vcm => new {
                MappingId = vcm.Id, 
                categoryId = vcm.Category!.Id!, 
                CategoryName = vcm.Category.Name
            }).ToList();
            return response;
        }

        public bool toggleStatus(int vendorId,List<int> categoryIds,bool status)
        {
            var mappingsToUpdate = _db.VendorCategoryMappings.Where(v => v.VendorId == vendorId && categoryIds.Contains(v.CategoryId ?? -1));
            foreach(var mapping in mappingsToUpdate)
            {              
                mapping.Status = status;
            }
            int change = _db.SaveChanges();
            return change > 0;
        }




    }
}
