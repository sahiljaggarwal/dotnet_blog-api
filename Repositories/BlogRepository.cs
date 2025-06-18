using BlogPortal.Data;
using BlogPortal.Dtos;
using BlogPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPortal.Repositories
{
    public class BlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Blog>> FindByAuthorIdAsync(int authorId)
        {
            return await _context.Blogs
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<List<Blog>> FindMany(QueryBlogDto queryDto)
        {
            int page = queryDto.Page;
            int limit = queryDto.Limit;
            string? search = queryDto.Search;
            var query = _context.Blogs.Include(b => b.Author).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(blog => blog.Title.Contains(search));
            }
            return await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
        }

        public async Task<int> CountBlogs(string? search = null)
        {
            var query = _context.Blogs.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(blog => blog.Title.Contains(search));
            }
            return await query.CountAsync();
        }

        public async Task<Blog?> FindByIdAsync(int id)
        {
            return await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Blog blog)
        {
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }

    }
}
