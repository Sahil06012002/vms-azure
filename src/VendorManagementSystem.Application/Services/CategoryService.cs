using Microsoft.IdentityModel.Tokens;
using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IErrorLoggingService _errorLog;
        private readonly ITokenService _tokenService;

        public CategoryService(ICategoryRepository categoryRepository, IErrorLoggingService errorLog, ITokenService tokenService)
        {
            _categoryRepository = categoryRepository;
            _errorLog = errorLog;
            _tokenService = tokenService;
        }

        public ApplicationResponseDto<Category> CreateCategory(CategoryDto categoryDto, string token)
        {
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string> { $"The jwt token is null" },
                        }
                    };
                }
                if (_categoryRepository.hasCategoryByName(categoryDto.Name))
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.DuplicateEntryError,
                            Message = new List<string> { $"Category Name: {categoryDto.Name} already present" },
                        }
                    };
                }
                var currentUser = _tokenService.ExtractUserDetials(token!, "id");
                Category category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    Status = true,
                    CreatedBy = Int32.Parse(currentUser),
                    UpdatedBy = Int32.Parse(currentUser),

                };
                var result = _categoryRepository.AddCategory(category);
                if (result != null)
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.DatabaseError,
                            Message = new List<string> { "Error While adding entry to Database" },
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);

                return new ApplicationResponseDto<Category>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }

        public ApplicationResponseDto<object> DeleteCategory(int id)
        {
            try
            {
                if (_categoryRepository.IsCateogoryUsed(id))
                {
                    throw new InvalidOperationException($"Category can't be deleted as is associated with a vendor");
                }
                var result = _categoryRepository.DeleteCategory(id);
                if (result == 1)
                {
                    return new ApplicationResponseDto<object>
                    {
                        Data = null,
                        Message = "Deletion is successful",
                    };
                }
                else
                {
                    return new ApplicationResponseDto<object>
                    {
                        Data = null,
                        Message = "No category found to delete",
                    };
                }
            }
            catch (InvalidOperationException ex)
            {
                _errorLog.LogError((int)ErrorCodes.InvalidOperation, ex);
                return new ApplicationResponseDto<object>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InvalidOperation),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<object>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }

        public ApplicationResponseDto<PagenationResponseDto> GetAllCategories(PaginationDto paginationDto, string? filter)
        {
            Console.WriteLine("{0} {1} {2}", paginationDto.Cursor, paginationDto.Size, paginationDto.Next);
            try
            {
                if (paginationDto.Cursor < 0 || paginationDto.Size <= 0)
                {
                    return new ApplicationResponseDto<PagenationResponseDto>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string>() { $"Inputs {paginationDto.Cursor} or {paginationDto.Size} is invalid. 'LastId should not be negative and Pagesize should be greater than 0'" }
                        }
                    };
                }

                IEnumerable<Category> categories = _categoryRepository.GetAllCategories(paginationDto, filter);


                int currentLastId = categories.Any() ? categories.ElementAt(categories.Count() - 1).Id : 0;
                int currentFirstId = categories.Any() ? categories.ElementAt(0).Id : 0;

                var result = categories.Select(category => new
                {
                    category.Id,
                    category.Name,
                    category.Description,
                    category.Status,
                    IsUsed = _categoryRepository.IsCateogoryUsed(category.Id),

                });
                return new ApplicationResponseDto<PagenationResponseDto>()
                {
                    Data = new PagenationResponseDto
                    {
                        PagenationData = result,
                        Cursor = currentLastId,
                        PreviousCursor = currentFirstId,
                        /*Size = categories.Count(),*/
                        HasNextPage = (currentLastId!=0) && _categoryRepository.NeighbourExsistance(currentLastId, true),
                        HasPreviousPage = (currentFirstId!=0) && _categoryRepository.NeighbourExsistance(currentFirstId, false),
                    }
                };

            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<PagenationResponseDto>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }

        public ApplicationResponseDto<Category> GetCategoryById(int id)
        {
            try
            {
                Category? category = _categoryRepository.GetCategory(id);
                if (category == null)
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.NotFound,
                            Message = new List<string> { $"Category with id {id} not found" },
                        }
                    };
                }
                else
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Data = category,
                    };
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<Category>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }
        }

        public ApplicationResponseDto<Category> UpdateCategoryById(int id, CategoryDto categoryDto, string token)
        {
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string> { $"The jwt token is null" },
                        }
                    };
                }
                var currentUser = _tokenService.ExtractUserDetials(token!, "id");
                CategoryUpdateDto categoryUpdate = new()
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    UpdatedBy = Int32.Parse(currentUser),
                };
                var res = _categoryRepository.UpdateCategory(id, categoryUpdate);
                if (res != null)
                {
                    return new ApplicationResponseDto<Category>
                    {
                        Data = res,
                    };
                }
                return new ApplicationResponseDto<Category>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.DatabaseError,
                        Message = new List<string> { "Error While adding entry to Database" },
                    }
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDto<Category>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }
    }
}
