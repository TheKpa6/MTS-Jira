using Microsoft.EntityFrameworkCore;
using MTSJira.Domain.Entities;

namespace MTSJira.Infrastructure.Database.Contexts
{
    public class JiraDbContext : DbContext
    {
        public JiraDbContext(DbContextOptions<JiraDbContext> options) : base(options) { }

        public DbSet<TaskData> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskData>()
                .HasOne(t => t.ParentTask)
                .WithMany(t => t.Subtasks)
                .HasForeignKey(t => t.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
