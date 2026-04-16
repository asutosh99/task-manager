
using Microsoft.EntityFrameworkCore;
using task_manager.Data;
using task_manager.DTO;
using task_manager.Models;
using task_manager.Interfaces;

namespace task_manager.Services
{
    public class TaskService: ITaskService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public TaskService(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<(List<TaskDTO>,int TotalCount)> GetTasksByUser(
            int page, 
            int pageSize,
            string? status,
            string? sortBy,
            string? order)
        {
            var userId = _currentUserService.UserId;

            if(userId == null) {
                throw new UnauthorizedAccessException("User not authorized");
            }

            var query = _context.Tasks.Where(t => t.UserId == userId).AsQueryable();
            //Filtering 
            if(!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }
            //sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower()=="title")
                {
                    query = order == "desc" ?
                        query.OrderByDescending(t => t.Title) :
                        query.OrderBy(t => t.Title);
                }
                else if (sortBy.ToLower() == "status")
                {
                    query = order == "desc"
                        ? query.OrderByDescending(t => t.Status)
                        : query.OrderBy(t => t.Status);
                }
                else
                {
                    query = query.OrderBy(t => t.Id);
                }

            }
            var  totalCounts= await query.CountAsync();
                
                var tasks =await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize).Select(t=> new TaskDTO
                {
                    Id=t.Id,
                    Status=t.Status,
                    Title=t.Title
                }).ToListAsync();
            return (tasks,totalCounts);
        }

        public async Task<TaskDTO> GetTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }
            if (task.UserId != _currentUserService.UserId && _currentUserService.UserRole != "Admin")
                throw new UnauthorizedAccessException("You cannot access this task");

           
            return MapToDto(task);
        }

        public async Task<TaskDTO> CreateTask(CreateTaskDto dto)
        {
            var userId = _currentUserService.UserId;

            if(userId == null) {
                throw new UnauthorizedAccessException("User not authorized");
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                UserId = (int)userId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<TaskDTO> Update(int id, UpdateTaskDto updatedTask)
        {
            var existing = await _context.Tasks.FindAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Task not found");

            if (existing.UserId != _currentUserService.UserId && _currentUserService.UserRole != "Admin")
                throw new UnauthorizedAccessException("You cannot update this task");

            existing.Title = updatedTask.Title;
            existing.Description = updatedTask.Description;
            existing.Status = updatedTask.Status;

            await _context.SaveChangesAsync();

            return MapToDto(existing);
        }

        public async Task DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) { 
                throw new KeyNotFoundException("Task not found");
            }
            if (task.UserId != _currentUserService.UserId && _currentUserService.UserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You cannot delete this task");
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            
        }

        private TaskDTO MapToDto(TaskItem task)
        {
            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status
            };
        }
    }
}
