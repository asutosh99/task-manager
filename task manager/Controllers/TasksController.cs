
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using task_manager.DTO;
using task_manager.Interfaces;
using task_manager.Models;
using task_manager.Services;


namespace task_manager.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
  
        private readonly ITaskService _taskService;
        private readonly CurrentUserService _currentUser;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, CurrentUserService currentUser, ILogger<TasksController> logger)
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
            var userId = _currentUser.UserId;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var (tasks,totalCounts) = await _taskService.GetTasksByUser( page, pageSize, status, sortBy, order);
            var response = new PagedResponse<List<TaskDTO>>
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Data = tasks,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCounts,
                TotalPages = (int)Math.Ceiling((double)totalCounts / pageSize)
            };
            _logger.LogInformation("Tasks retrieved successfully for user {UserId}", userId);
            return Ok(response);
        }

        [Authorize]
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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> Update(int id, UpdateTaskDto dto)
        {

            var task =await _taskService.Update(id,dto);
          _logger.LogInformation("Task with id {TaskId} updated successfully", id);
            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "Task is updated",
                Data = task
            });
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> GetById(int id)
        {
            var task = await _taskService.GetTaskById(id);
      
         _logger.LogInformation("Task with id {TaskId} retrieved successfully", id);
            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "found the Task successfully",
                Data = task
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            await _taskService.DeleteTask(id);
            _logger.LogInformation("Task with id {TaskId} deleted successfully", id);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Task deleted",
                Data = "deleted"
            });

        }
    }
}
