namespace task_manager.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? UserRole { get; }

    }
}
