using System.ComponentModel.DataAnnotations;

namespace task_manager.DTO
{
    public class LoginDto
    {
        [Required]
        
        public string Email { get; set; }

        [Required] public string Password { get;set; }
    }
}
