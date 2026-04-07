using Microsoft.AspNetCore.Mvc;
using task_manager.DTO;
using task_manager.Models;
using task_manager.Services;


namespace task_manager.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
  
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskDTO>>> GetAll()
        {
            var tasks = await _taskService.GetTasks();
            return Ok(tasks);
        }
        [HttpPost]
        public async Task<ActionResult<TaskDTO>> Create(CreateTaskDto dto)
        {
           
            TaskItem newTask = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status
            };
            var created = await _taskService.CreateTask(newTask);
            return Ok(created);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDTO>> Update(int id, UpdateTaskDto dto)
        {

            var updatedTask = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status
            };

            var task =await _taskService.Update(id,updatedTask);
            if(task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDTO>> GetById(int id)
        {
            var task = await _taskService.GetTaskById(id);
         if(task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var sucess= await _taskService.DeleteTask(id);
            if(sucess == false)
            {
                return NotFound();
            }
            return Ok(sucess);

        }
    }
}
