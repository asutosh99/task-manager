using task_manager.DTO;


namespace task_manager.Interfaces
{
    public interface ITaskService
    {
        Task<(List<TaskDTO>, int TotalCount)> GetTasksByUser(
             int page,
             int pageSize,
             string? status,
             string? sortBy,
             string? order);
        Task<TaskDTO> GetTaskById(int id);
        Task<TaskDTO> CreateTask(CreateTaskDto dto);
        Task<TaskDTO> Update(int id, UpdateTaskDto updatedTask);
        Task DeleteTask(int id);
    }
}