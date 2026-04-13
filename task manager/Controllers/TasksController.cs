
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly CurrentUserService _currentUser;
        private readonly ILogger<TasksController> _logger;

        public TasksController(TaskService taskService, CurrentUserService currentUser, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _currentUser = currentUser;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResponse<List<TaskDTO>>>> GetAll(
            int page=1, 
            int pageSize=10,
            string? status=null, 
            string? sortBy=null, 
            string? order="asc")
        {
            
            if (_currentUser.UserId == null)
            {
                _logger.LogWarning("Unauthorized access attempt to GetAll tasks");
                return Unauthorized();
            }
            var userId = _currentUser.UserId.Value;

            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var (tasks,totalCounts) = await _taskService.GetTasksByUser(userId, page, pageSize, status, sortBy, order);
            var response = new PagedResponse<List<TaskDTO>>
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Data = tasks,
                Page = page,
                TotalCount = totalCounts,
                TotalPages = (int)Math.Ceiling((double)totalCounts / pageSize)
            };
            _logger.LogInformation("Tasks retrieved successfully for user {UserId}", userId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> Create(CreateTaskDto dto)
        {
            var userId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            if (userId == null) { return Unauthorized(); }
            var created = await _taskService.CreateTask(dto, int.Parse(userId));
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
                _logger.LogWarning("Task with id {TaskId} not found", id);
                return NotFound(new ApiResponse<TaskDTO>
                {
                    Success = false,
                    Message = "Task not found",
                    Data = null
                });
            }
         _logger.LogInformation("Task with id {TaskId} retrieved successfully", id);
            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "found the Task successfully",
                Data = task
            });
        }

        [Authorize(Roles = "Admin")]
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
