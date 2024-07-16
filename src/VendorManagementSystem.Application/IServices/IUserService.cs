using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IUserService
    {
        ApplicationResponseDto<User> CreateUser(CreateUserDto userDto, string currentUser);
        ApplicationResponseDto<TokenDto> Login(LoginDto loginDto);
        User CreateSuperAdmin(SuperAdminDto superAdminDto);
        ApplicationResponseDto<bool> SetUserPassword(UpdatePasswordDto updatePasswordDto, string _token);
        ApplicationResponseDto<string> ValidateToken(string token);
        ApplicationResponseDto<bool> SendForgetPasswordEmail(string email, string redirectUrl);
        ApplicationResponseDto<PagenationResponseDto> GetAllUsers(PaginationDto paginationDto, string? filter);
    }
}
