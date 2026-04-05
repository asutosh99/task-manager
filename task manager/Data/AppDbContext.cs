using task_manager.Models;
using Microsoft.EntityFrameworkCore;


namespace task_manager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Task> Tasks { get; set; }
    }
}
