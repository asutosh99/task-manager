using BCrypt.Net;
using Microsoft.EntityFrameworkCore;    
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using task_manager.Data;
using task_manager.DTO;
using task_manager.Interfaces;
using task_manager.Models;
using static task_manager.DTO.AuthResponseDto;

namespace task_manager.Services
{
    public class AuthService: IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<bool> Register(RegisterDto registerDto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return false; // User already exists
            }
            // Create new user
            var user = new User
            {
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Role, user.Role)
            };

            //  Create key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token = tokenString,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email
                }
            };
        }
    }
}
