using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BlogPortal.Dtos;
using BlogPortal.Models;
using BlogPortal.Repositories;
using BlogPortal.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BlogPortal.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;
        private readonly IConfiguration _config;

        public UserService(UserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<ApiResponse<object>> SignUp(SignUpDto dto)
        {
            var existing = await _userRepo.FindByEmail(dto.Email);
            if (existing != null)
            {
                return new ApiResponse<object>(false, "Email already in user");
            }
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                Role = Enum.TryParse<Role>(dto.Role, out var role) ? role : Role.READER
            };

            await _userRepo.Create(user);
            return new ApiResponse<object>(true, "SignUp Successfull", new { user.Id, user.Email, user.Username, user.Role });
        }

        public async Task<ApiResponse<List<UserResponseDto>>> GetAllUsers(QueryUserDto query)
        {
            var users = await _userRepo.FindMany(query);
            var total = await _userRepo.CountUsers(query.Search);
            if (users == null)
            {
                return new ApiResponse<List<UserResponseDto>>(false, "Users not found", null);
            }
            var userDtos = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.Username,
                Role = u.Role.ToString()
            }).ToList();
            var totalPages = (int)Math.Ceiling((double)total / query.Limit);

            var meta = new
            {
                total,
                totalPages,
                currentPage = query.Page,
                limit = query.Limit
            };
            return new ApiResponse<List<UserResponseDto>>(true, "Users fetched successfully", userDtos, meta);
        }

        public async Task<ApiResponse<object>> Login(LoginDto dto)
        {
            var user = await _userRepo.FindByEmailAndPassword(dto.Email, dto.Password);
            if (user == null)
            {
                return new ApiResponse<object>(false, "Invalid credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var data = new
            {
                token = tokenString,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.Username,
                    Role = user.Role.ToString()
                }
            };

            return new ApiResponse<object>(true, "Login successfull", data);
        }
    }
}