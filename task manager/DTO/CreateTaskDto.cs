using System.ComponentModel.DataAnnotations;

namespace task_manager.DTO
{
    public class CreateTaskDto
    {
        [Required]
        [MinLength(3)]

        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
