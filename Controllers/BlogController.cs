
using BlogPortal.Dtos;
using BlogPortal.Models;
using BlogPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogPortal.Controllers
{
    [ApiController]
    [Route("api/blogs")]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        // Create Blog Controller
        [HttpPost]
        [AllowAuthenticated]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogDto dto, [GetUser] UserPayload user)
        {
            var result = await _blogService.CreateBlogAsync(dto, user.Id!);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Get Blogs Controller
        [HttpGet("my")]
        [AllowAuthenticated]
        public async Task<IActionResult> GetMyBlogs([GetUser] UserPayload user)
        {
            var result = await _blogService.GetBlogsByUserAsync(user.Id!);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        [AllowAuthenticated]
        public async Task<IActionResult> GetBlogs([FromQuery] QueryBlogDto query)
        {
            var result = await _blogService.GetAllBlogs(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // Get Blog Controller
        [HttpGet("{id}")]
        [AllowAuthenticated]
        public async Task<IActionResult> GetBlogById(int id, [GetUser] UserPayload user)
        {
            var result = await _blogService.GetBlogByIdAsync(id, user.Id!);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // Update Blog Controller
        [HttpPut("{id}")]
        [AllowAuthenticated]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] CreateBlogDto dto, [GetUser] UserPayload user)
        {
            var result = await _blogService.UpdateBlogAsync(id, dto, user.Id!);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Delete Blog Controller
        [HttpDelete("{id}")]
        [AllowAuthenticated]
        public async Task<IActionResult> DeleteBlog(int id, [GetUser] UserPayload user)
        {
            var result = await _blogService.DeleteBlogAsync(id, user.Id!);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
