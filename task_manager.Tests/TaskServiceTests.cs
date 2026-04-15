using Xunit;
using Microsoft.EntityFrameworkCore;
using task_manager.Data;
using task_manager.Services;
using task_manager.Models;
using task_manager.DTO;
using FluentAssertions;
using Moq;
using task_manager.Interfaces;

public class TaskServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
    [Fact]
    public async Task CreateTask_Should_Create_Task()
    {
        var context = GetDbContext();

        var user = new Mock<ICurrentUserService>();
        user.Setup(x => x.UserId).Returns(1);
        user.Setup(x => x.UserRole).Returns("Admin");

        var service = new TaskService(context, user.Object);

        var dto = new CreateTaskDto
        {
            Title = "Test",
            Description = "Desc",
            Status = "Pending"
        };

        var result = await service.CreateTask(dto, 1);

        result.Should().NotBeNull();
        context.Tasks.Count().Should().Be(1);
    }

    [Fact]
    public async Task Update_Should_Throw_When_NotOwner()
    {
        var context = GetDbContext();

        context.Tasks.Add(new TaskItem
        {
            Title = "Test",
            Description = "Desc",
            Status = "Pending",
            UserId = 2
        });
        await context.SaveChangesAsync();

        var user = new Mock<ICurrentUserService>();
        user.Setup(x => x.UserId).Returns(1);
        user.Setup(x => x.UserRole).Returns("User");

        var service = new TaskService(context, user.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.Update(1, new UpdateTaskDto()));
    }

    [Fact]
    public async Task Delete_Should_Work_For_Admin()
    {
        var context = GetDbContext();

        var task = new TaskItem
        {
            Title = "Test",
            Description = "Desc",
            Status = "Pending",
            UserId = 2
        };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var user = new Mock<ICurrentUserService>();
        user.Setup(x => x.UserId).Returns(1);
        user.Setup(x => x.UserRole).Returns("Admin");

        var service = new TaskService(context, user.Object);

        var result = await service.DeleteTask(task.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetTaskById_Should_Return_Null_When_NotFound()
    {
        var context = GetDbContext();

        var service = new TaskService(context, Mock.Of<ICurrentUserService>());

        var result = await service.GetTaskById(999);

        result.Should().BeNull();
    }
}