using task_manager.DTO;


namespace task_manager.Interfaces
{
    public interface ITaskService
    {
        Task<(List<TaskDTO>, int TotalCount)> GetTasksByUser(
             int userId,
             int page,
             int pageSize,
             string? status,
             string? sortBy,
             string? order);
        Task<TaskDTO> GetTaskById(int id);
        Task<TaskDTO> CreateTask(CreateTaskDto dto, int userId);
        Task<TaskDTO> Update(int id, UpdateTaskDto updatedTask);
        Task<bool> DeleteTask(int id);
    }
}