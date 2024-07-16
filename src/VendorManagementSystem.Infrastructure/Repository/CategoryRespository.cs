using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class CategoryRespository : ICategoryRepository
    {
        private readonly DataContext _db;

        public CategoryRespository(DataContext db)
        {
            _db = db;
        }
        public Category? AddCategory(Category category)
        {
            _db.Categories.Add(category);
            int result = _db.SaveChanges();
            if (result > 0)
            {
                return category;
            }
            return null;
        }

        public int DeleteCategory(int id)
        {
            int ans;
            var transaction = _db.Database.BeginTransaction();
            // removing foreign key dependency
            var mappings = _db.VendorCategoryMappings.Where(vcm => vcm.CategoryId == id).ToList();
            if (mappings != null)
            {
                foreach (var mapping in mappings)
                {
                    mapping.CategoryId = null;
                    mapping.Status = false;
                }
            }

            // now remove the category. 
            var category = _db.Categories.Where(c => c.Id == id).FirstOrDefault();
            if (category != null)
            {
                _db.Categories.Remove(category);
                ans = 1;
            }
            else
            {
                ans = 0;
            }
            _db.SaveChanges();
            transaction.Commit();
            return ans;
        }

        public IEnumerable<Category> GetAllCategories(PaginationDto paginationDto, string? filter)
        {
            int cursor = paginationDto.Cursor, pageSize = paginationDto.Size;
            bool next = paginationDto.Next;
            IQueryable<Category> categories = _db.Categories.Where(category => category.Status);
            Console.WriteLine("Whats hapenning???");
           if(next)
            {
                if (cursor != 0)
                {
                    categories = categories.Where(category => category.Id < cursor);
                }
                
                categories = categories.OrderByDescending(category => category.Id);
            }
            else
            {
                categories = _db.Categories.Where(category => category.Id > cursor).OrderBy(c => c.Id);
            }

            if(!string.IsNullOrWhiteSpace(filter)) 
            {
                categories = categories.Where(category => category.Name.Contains(filter));
            }
            categories = categories.Take(pageSize);
            var result = categories.ToList();

            if (!next)
            {
                result = result.OrderByDescending(c => c.Id).ToList();
            }

            return result;
        }
        public bool IsCateogoryUsed(int id)
        {
            var doesExist = _db.VendorCategoryMappings.Where(vcm => vcm.CategoryId == id).FirstOrDefault();
            return doesExist != null;
        }
        public Category? GetCategory(int id)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == id);
            return category;
        }

        public IEnumerable<VendorFormCategoriesDto> GetCategoriesForForm()
        {
            IEnumerable<VendorFormCategoriesDto> categories =
                _db.Categories.Where(c => c.Status)
                .Select(c => new VendorFormCategoriesDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
            return categories;
        }

        public bool hasCategoryByName(string name)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Name == name);
            return category != null;
        }

        public Category? UpdateCategory(int id, CategoryUpdateDto categoryDto)
        {
            Category? category = GetCategory(id);
            if (category == null) throw new Exception($"Can't find a category of id {id}");

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.UpdatedAt = categoryDto.UpdatedAt;
            category.UpdatedBy = categoryDto.UpdatedBy;

            int res = _db.SaveChanges();
            if (res > 0)
            {
                return GetCategory(id);
            }
            return null;
        }

        public bool NeighbourExsistance(int id, bool next)
        {
            IQueryable<Category> query = _db.Categories.OrderByDescending(c => c.Id);
            bool result = next ? query.Any(c => c.Id < id) : query.Any(c => c.Id > id);
            return result;
        }
    }
}
