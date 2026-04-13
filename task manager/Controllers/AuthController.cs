using Microsoft.AspNetCore.Mvc;
using task_manager.DTO;
using task_manager.Models;
using task_manager.Services;


namespace task_manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }
     
        [HttpPost("resgister")]
        public async Task<ActionResult<ApiResponse<string>>> Register(RegisterDto registerDto)
        {

            var result = await _authService.Register(registerDto);
            if (result == "User already exists")
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "User already exist",
                    Data = null
                });
            }
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "User registered successfully",
                Data = result
            }
                );

        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto loginDto)
        {
            var token = await _authService.Login(loginDto, _configuration);
            if (token == null)
            {
                return Unauthorized(new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Data = null
                });
            }

            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = token
            });
        }
    }
}
