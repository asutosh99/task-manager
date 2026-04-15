using task_manager.DTO;

namespace task_manager.Interfaces
{
    public interface IAuthService
    {

        Task<bool> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto dto);

    }
}
