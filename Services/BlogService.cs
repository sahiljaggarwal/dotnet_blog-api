using BlogPortal.Dtos;
using BlogPortal.Models;
using BlogPortal.Repositories;
using BlogPortal.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlogPortal.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepo;

        public BlogService(BlogRepository blogRepo)
        {
            _blogRepo = blogRepo;
        }

        // Add Blog Service
        public async Task<ApiResponse<object>> CreateBlogAsync(CreateBlogDto dto, string userId)
        {
            var blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = int.Parse(userId)
            };

            await _blogRepo.CreateAsync(blog);
            return new ApiResponse<object>(true, "Blog created successfully", new { blog.Id });
        }

        // Get User Blog Service
        public async Task<ApiResponse<List<Blog>>> GetBlogsByUserAsync(string userId)
        {
            if (userId == null)
            {
                return new ApiResponse<List<Blog>>(false, "UserId is required", null);
            }
            var blogs = await _blogRepo.FindByAuthorIdAsync(int.Parse(userId));
            if (blogs == null || blogs.Count == 0)
            {
                return new ApiResponse<List<Blog>>(false, "No blogs found for user");
            }

            return new ApiResponse<List<Blog>>(true, "Blogs fetched successfully", blogs);
        }

        // Get Users Blog Service
        public async Task<ApiResponse<object>> GetAllBlogs([FromQuery] QueryBlogDto query)
        {
            var blogs = await _blogRepo.FindMany(query);
            var total = await _blogRepo.CountBlogs(query?.Search);
            if (blogs == null)
            {
                return new ApiResponse<object>(false, "User not found", null);
            }
            var blogDtos = blogs.Select(b => new
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                Auther = new { id = b.Author.Id, username = b.Author.Username },
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            }).ToList();
            var totalPages = (int)Math.Ceiling((double)total / query.Limit);

            var meta = new
            {
                total,
                totalPages,
                currentPage = query.Page,
                limit = query.Limit
            };
            return new ApiResponse<object>(true, "Blogs fetched successfully", blogDtos, meta);
        }

        // Get Blog By Id Service
        public async Task<ApiResponse<Blog>> GetBlogByIdAsync(int id, string userId)
        {
            var blog = await _blogRepo.FindByIdAsync(id);
            if (blog == null || blog.AuthorId != int.Parse(userId))
            {
                return new ApiResponse<Blog>(false, "Blog not found or unauthorized");
            }

            return new ApiResponse<Blog>(true, "Blog fetched successfully", blog);
        }

        // Update Blog Service
        public async Task<ApiResponse<object>> UpdateBlogAsync(int id, CreateBlogDto dto, string userId)
        {
            var blog = await _blogRepo.FindByIdAsync(id);
            if (blog == null || blog.AuthorId != int.Parse(userId))
            {
                return new ApiResponse<object>(false, "Blog not found or unauthorized");
            }

            blog.Title = dto.Title;
            blog.Content = dto.Content;

            await _blogRepo.UpdateAsync(blog);
            return new ApiResponse<object>(true, "Blog updated successfully");
        }

        // Delete Blog Service
        public async Task<ApiResponse<object>> DeleteBlogAsync(int id, string userId)
        {
            var blog = await _blogRepo.FindByIdAsync(id);
            if (blog == null || blog.AuthorId != int.Parse(userId))
            {
                return new ApiResponse<object>(false, "Blog not found");
            }

            await _blogRepo.DeleteAsync(blog);
            return new ApiResponse<object>(true, "Blog deleted successfully");
        }
    }
}
