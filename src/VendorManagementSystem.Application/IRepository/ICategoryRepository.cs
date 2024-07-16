using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface ICategoryRepository
    {
        public Category? AddCategory(Category category);
        public Category? GetCategory(int id);
        public IEnumerable<Category> GetAllCategories(PaginationDto paginationDto, string? filter);
        public int DeleteCategory(int id);
        public bool hasCategoryByName(string name);
        public Category? UpdateCategory(int id, CategoryUpdateDto categoryDto);
        public IEnumerable<VendorFormCategoriesDto> GetCategoriesForForm();
        public bool IsCateogoryUsed(int id);

        public bool NeighbourExsistance(int id, bool next);
    }
}
