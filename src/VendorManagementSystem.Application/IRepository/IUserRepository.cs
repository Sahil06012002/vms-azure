using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IUserRepository
    {
        IDbContextTransaction BeginTransaction();
        int AddUser(User user);
        User? GetUserByEmail(string email);
        public IEnumerable<User> GetUsers(PaginationDto paginationDto, string? filter);
        bool UpdatePassword(string email, string newPassword);
        public bool NeighbourExistance(int id, bool next);
    }
}