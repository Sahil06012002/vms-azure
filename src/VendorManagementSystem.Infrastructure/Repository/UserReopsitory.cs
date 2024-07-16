using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class UserReopsitory : IUserRepository
    {
        private readonly DataContext _db;

        public UserReopsitory(DataContext db, IErrorLoggingService errorLog)
        {
            _db = db;
        }
        public int AddUser(User user)
        {
            _db.Users.Add(user);
            return _db.SaveChanges();
        }

        public User? GetUserByEmail(string email)
        {
            User? user = _db.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public bool NeighbourExistance(int id, bool next)
        {
            IQueryable<User> query = _db.Users.OrderByDescending(u => u.Id);

            return next ? query.Any(u => u.Id < id && !u.Role.Equals("superadmin")) : query.Any(u => u.Id > id && !u.Role.Equals("superadmin"));
        }

        public IEnumerable<User> GetUsers(PaginationDto paginationDto, string? filter)
        {
            int cursor = paginationDto.Cursor, size = paginationDto.Size;
            bool next = paginationDto.Next;
            IQueryable<User> query;
            if (next)
            {
                if (cursor == 0)
                {
                    query = _db.Users.OrderByDescending(u => u.Id).Where(u => u.Role.Equals("admin"));
                }
                else
                {
                    query = _db.Users.OrderByDescending(u => u.Id).Where(u => u.Id < cursor && u.Role.Equals("admin"));
                }
            }else
            {
                query = _db.Users.OrderBy(u => u.Id).Where(u => u.Id > cursor && u.Role.Equals("admin"));
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                Console.WriteLine($"Getting users with filter {filter}");
                query = query.Where(u => u.UserName.Contains(filter) || u.Email.Contains(filter));
            }

            var result = query
                .Take(size)
                .Select(user => new User
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.CreatedBy,
                    UpdatedAt = user.UpdatedAt,
                    UpdatedBy = user.UpdatedBy,
                    Status = user.Status,
                }).ToList();

            if(next) return result;

            return result.OrderByDescending(u => u.Id).ToList();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            User? user = _db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return false;
            }
            user.Status = true;
            user.Password = newPassword;
            user.UpdatedBy = user.Id;
            user.UpdatedAt = DateTime.UtcNow;
            int changes = _db.SaveChanges();

            return changes > 0;
        }
    }
}
