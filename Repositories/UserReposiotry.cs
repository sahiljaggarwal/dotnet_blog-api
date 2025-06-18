
using BlogPortal.Data;
using BlogPortal.Dtos;
using BlogPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPortal.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> FindMany(QueryUserDto queryDto)
        {
            int page = queryDto.Page;
            int limit = queryDto.Limit;
            string? search = queryDto.Search;

            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Username.Contains(search) || u.Email.Contains(search));
            }
            return await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
        }

        public async Task<int> CountUsers(string? search = null)
        {
            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Username.Contains(search) || u.Email.Contains(search));

            }
            return await query.CountAsync();
        }

        public async Task<User?> FindByEmailAndPassword(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}