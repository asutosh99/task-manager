
using Microsoft.EntityFrameworkCore;
using task_manager.Data;
using task_manager.DTO;
using task_manager.Models;

namespace task_manager.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;
        public TaskService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<(List<TaskDTO>,int TotalCount)> GetTasks(int page, int pageSize,string? status)
        {
            
            var query = _context.Tasks.AsQueryable();
            
            if(!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
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
            //throw new Exception("Test logging error");
            var task = await _context.Tasks.FindAsync(id);

            if (task == null) return null;

            return MapToDto(task);
        }

        public async Task<TaskDTO> CreateTask(CreateTaskDto dto)
        {

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<TaskDTO> Update(int id, UpdateTaskDto updatedTask)
        {
            var existing = await _context.Tasks.FindAsync(id);

            if (existing == null)
                return null;

            existing.Title = updatedTask.Title;
            existing.Description = updatedTask.Description;
            existing.Status = updatedTask.Status;

            await _context.SaveChangesAsync();

            return MapToDto(existing);
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
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
