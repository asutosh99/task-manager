using System.Security.Claims;
using task_manager.Interfaces;
namespace task_manager.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var UserId = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return UserId != null ? int.Parse(UserId) : null;
            }
        }

        public string? UserRole
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.Role)?.Value;
            }
        }
    }
}
