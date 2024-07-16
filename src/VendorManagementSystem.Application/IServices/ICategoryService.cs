using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface ICategoryService
    {
        public ApplicationResponseDto<Category> CreateCategory(CategoryDto categoryDto, string token);
        public ApplicationResponseDto<PagenationResponseDto> GetAllCategories(PaginationDto paginationDto, string? filter);
        public ApplicationResponseDto<Category> GetCategoryById(int id);
        public ApplicationResponseDto<Category> UpdateCategoryById(int id, CategoryDto categoryDto, string token);

        public ApplicationResponseDto<object> DeleteCategory(int id);
    }
}
