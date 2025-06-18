
using BlogPortal.Dtos;
using BlogPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogPortal.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService service)
        {
            _userService = service;

        }

        // Signup Users Controller
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            var result = await _userService.SignUpAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Find All Users Controller
        [HttpGet]
        public async Task<IActionResult> FindAllUsers([FromQuery] QueryUserDto query)
        {
            var result = await _userService.GetAllUsersAsync(query);
            return result.Success ? Ok(result) : NotFound(result);

        }

        // Login User Controller
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _userService.Login(dto);
            if (!result.Success || result.Data == null)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
