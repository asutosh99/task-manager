using Microsoft.AspNetCore.Mvc;
using task_manager.DTO;
using task_manager.Interfaces;
using task_manager.Models;
using task_manager.Services;


namespace task_manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
     
        [HttpPost("resgister")]
        public async Task<ActionResult<ApiResponse<string>>> Register(RegisterDto registerDto)
        {

            var result = await _authService.Register(registerDto);
           
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "User registered successfully",
                Data = "User registered successfully"
            }
                );

        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto loginDto)
        {
            var token = await _authService.Login(loginDto);
           
            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = token
            });
        }
    }
}
