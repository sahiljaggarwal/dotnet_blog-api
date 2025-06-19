using BlogPortal.Dtos;
using BlogPortal.Models;
using BlogPortal.Repositories;
using BlogPortal.Shared;

namespace BlogPortal.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepo;

        public BlogService(BlogRepository blogRepo)
        {
            _blogRepo = blogRepo;
        }

        public async Task<ApiResponse<object>> CreateBlogAsync(CreateBlogDto dto, int userId)
        {
            var blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = userId,
                MediaFileId = dto.MediaFileId
            };

            await _blogRepo.CreateAsync(blog);
            return new ApiResponse<object>(true, "Blog created successfully", new { blog.Id });
        }

        public async Task<ApiResponse<List<Blog>>> GetBlogsByUserAsync(int userId)
        {

            var blogs = await _blogRepo.FindByAuthorIdAsync(userId);
            if (blogs.Count == 0)
                return new ApiResponse<List<Blog>>(false, "No blogs found for user");

            return new ApiResponse<List<Blog>>(true, "Blogs fetched successfully", blogs);
        }

        public async Task<ApiResponse<object>> GetAllBlogs(QueryBlogDto query)
        {
            var blogs = await _blogRepo.FindMany(query);
            var total = await _blogRepo.CountBlogs(query?.Search);

            var blogDtos = blogs.Select(b => new
            {
                b.Id,
                b.Title,
                b.Content,
                Author = new { b.Author.Id, b.Author.Username },
                MediaFile = b.MediaFile != null ? new { b.MediaFile.Id, b.MediaFile.Url } : null,
                b.CreatedAt,
                b.UpdatedAt
            }).ToList();

            var limit = query?.Limit ?? 10;
            var skip = query?.Page ?? 1;
            var totalPages = (int)Math.Ceiling((double)total / limit);

            var meta = new
            {
                total,
                totalPages,
                currentPage = skip,
                limit
            };

            return new ApiResponse<object>(true, "Blogs fetched successfully", blogDtos, meta);
        }

        public async Task<ApiResponse<Blog>> GetBlogByIdAsync(int id, int userId)
        {
            var blog = await _blogRepo.FindByIdAsync(id);
            if (blog == null || blog.AuthorId != userId)
            {
                return new ApiResponse<Blog>(false, "Blog not found or unauthorized");
            }

            return new ApiResponse<Blog>(true, "Blog fetched successfully", blog);
        }

        public async Task<ApiResponse<object>> UpdateBlogAsync(int id, CreateBlogDto dto, int userId)
        {
            var blog = await _blogRepo.FindByIdAsync(id);
            if (blog == null || blog.AuthorId != userId)
            {
                return new ApiResponse<object>(false, "Blog not found or unauthorized");
            }

            blog.Title = dto.Title;
            blog.Content = dto.Content;
            blog.MediaFileId = dto.MediaFileId;

            await _blogRepo.UpdateAsync(blog);
            return new ApiResponse<object>(true, "Blog updated successfully");
        }

        public async Task<ApiResponse<object>> DeleteBlogAsync(int id, int userId)
        {
            var blog = await _blogRepo.FindByIdAsync(id);
            if (blog == null || blog.AuthorId != userId)
            {
                return new ApiResponse<object>(false, "Blog not found or unauthorized");
            }

            await _blogRepo.DeleteAsync(blog);
            return new ApiResponse<object>(true, "Blog deleted successfully");
        }
    }
}
