
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
        public async Task<ActionResult<ApiResponse<List<TaskDTO>>>> GetAll()
        {
            var tasks = await _taskService.GetTasks();
            var response = new ApiResponse<List<TaskDTO>>
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Data = tasks
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> Create(CreateTaskDto dto)
        {
           
            
            var created = await _taskService.CreateTask(dto);
            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "Task created successfully",
                Data = created
            });
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> Update(int id, UpdateTaskDto dto)
        {

            var task =await _taskService.Update(id,dto);
            if(task == null)
            {
                return NotFound(new ApiResponse<TaskDTO>
                {
                    Success = false,
                    Message = "Task not found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "Task is updated",
                Data = task
            });
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> GetById(int id)
        {
            var task = await _taskService.GetTaskById(id);
         if(task == null)
            {
                return NotFound(new ApiResponse<TaskDTO>
                {
                    Success = false,
                    Message = "Task not found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "found the Task successfully",
                Data = task
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var sucess= await _taskService.DeleteTask(id);
            if(sucess == false)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success=false,
                    Message = "Task not found",
                    Data=null
                });
            }
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Task deleted",
                Data = null
            });

        }
    }
}
