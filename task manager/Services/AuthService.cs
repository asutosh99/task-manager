using BCrypt.Net;
using Microsoft.EntityFrameworkCore;    
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using task_manager.Data;
using task_manager.DTO;
using task_manager.Models;
using static task_manager.DTO.AuthResponseDto;

namespace task_manager.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> Register(RegisterDto registerDto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return null; // User already exists
            }
            // Create new user
            var user = new User
            {
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "User registered successfully";
        }

        public async Task<AuthResponseDto> Login(LoginDto dto, IConfiguration config)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            var Users = await _context.Users.Select(u => u.Email).ToListAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return null;
            }

            // Create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            //  Create key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
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
