// using BlogPortal.Data;
// using BlogPortal.Dtos;
// using BlogPortal.Models;
// using Microsoft.EntityFrameworkCore;

// namespace BlogPortal.Repositories
// {
//     public class BlogRepository
//     {
//         private readonly ApplicationDbContext _context;

//         public BlogRepository(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         public async Task CreateAsync(Blog blog)
//         {
//             _context.Blogs.Add(blog);
//             await _context.SaveChangesAsync();
//         }

//         public async Task<List<Blog>> FindByAuthorIdAsync(int authorId)
//         {
//             return await _context.Blogs
//                 .Where(b => b.AuthorId == authorId)
//                 .ToListAsync();
//         }

//         public async Task<List<Blog>> FindMany(QueryBlogDto queryDto)
//         {
//             int page = queryDto.Page;
//             int limit = queryDto.Limit;
//             string? search = queryDto.Search;
//             var query = _context.Blogs.Include(b => b.Author).Include(b => b.MediaFile).AsQueryable();
//             if (!string.IsNullOrEmpty(search))
//             {
//                 query = query.Where(blog => blog.Title.Contains(search));
//             }
//             return await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
//         }

//         public async Task<int> CountBlogs(string? search = null)
//         {
//             var query = _context.Blogs.AsQueryable();
//             if (!string.IsNullOrEmpty(search))
//             {
//                 query = query.Where(blog => blog.Title.Contains(search));
//             }
//             return await query.CountAsync();
//         }

//         public async Task<Blog?> FindByIdAsync(int id)
//         {
//             return await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
//         }

//         public async Task UpdateAsync(Blog blog)
//         {
//             _context.Blogs.Update(blog);
//             await _context.SaveChangesAsync();
//         }

//         public async Task DeleteAsync(Blog blog)
//         {
//             _context.Blogs.Remove(blog);
//             await _context.SaveChangesAsync();
//         }

//     }
// }


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
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Blog>> FindByAuthorIdAsync(int authorId)
        {
            return await _context.Blogs
                .AsNoTracking()
                .Where(b => b.AuthorId == authorId)
                .Include(b => b.MediaFile)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Blog>> FindMany(QueryBlogDto queryDto)
        {
            int page = Math.Max(1, queryDto.Page);
            int limit = Math.Max(1, queryDto.Limit);
            string? search = queryDto.Search;

            var query = _context.Blogs
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.MediaFile)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(blog => blog.Title.Contains(search));
            }

            return await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> CountBlogs(string? search = null)
        {
            var query = _context.Blogs.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(blog => blog.Title.Contains(search));
            }
            return await query.CountAsync();
        }

        public async Task<Blog?> FindByIdAsync(int id)
        {
            return await _context.Blogs
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.MediaFile)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Blog blog)
        {
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Blog blog)
        {
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
    }
}

