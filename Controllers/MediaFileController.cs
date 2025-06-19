using BlogPortal.Models;
using BlogPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogPortal.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [AllowAuthenticated]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [GetUser] UserPayload user)
        {
            var result = await _fileService.AddFileAsync(file, user.Id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
