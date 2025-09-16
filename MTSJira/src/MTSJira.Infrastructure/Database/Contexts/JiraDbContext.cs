using Microsoft.EntityFrameworkCore;
using MTSJira.Domain.Entities;

namespace MTSJira.Infrastructure.Database.Contexts
{
    public class JiraDbContext : DbContext
    {
        public JiraDbContext(DbContextOptions<JiraDbContext> options) : base(options) { }

        public DbSet<TaskData> Tasks { get; set; }
        public DbSet<TaskRelationshipData> TaskRelationships { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskData>()
                .HasOne(t => t.ParentTask)
                .WithMany(t => t.Subtasks)
                .HasForeignKey(t => t.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskRelationshipData>()
               .HasOne(tr => tr.SourceTask)
               .WithMany(t => t.RelatedTasks)
               .HasForeignKey(tr => tr.SourceTaskId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskRelationshipData>()
                .HasOne(tr => tr.RelatedTask)
                .WithMany(t => t.RelatedToTasks)
                .HasForeignKey(tr => tr.RelatedTaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
