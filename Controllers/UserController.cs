
using BlogPortal.Dtos;
using BlogPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogPortal.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _config;

        public UserController(UserService service)
        {
            _userService = service;

        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            var result = await _userService.SignUp(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> FindAllUsers([FromQuery] QueryUserDto query)
        {
            var result = await _userService.GetAllUsers(query);
            return result.Success ? Ok(result) : NotFound(result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _userService.Login(dto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            var token = ((dynamic)result.Data).token;

            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(result);
        }
    }
}
