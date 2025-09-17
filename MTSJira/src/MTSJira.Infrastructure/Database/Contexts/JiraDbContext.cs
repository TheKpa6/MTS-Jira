using Microsoft.EntityFrameworkCore;
using MTSJira.Domain.Entities;
using MTSJira.Infrastructure.Database.Configurations;

namespace MTSJira.Infrastructure.Database.Contexts
{
    public class JiraDbContext : DbContext
    {
        public JiraDbContext(DbContextOptions<JiraDbContext> options) : base(options) { }

        public DbSet<TaskData> Tasks { get; set; }
        public DbSet<TaskRelationshipData> TaskRelationships { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new TaskRelationshipConfiguration());
            modelBuilder.ApplyConfiguration(new TaskStatusConfiguration());
            modelBuilder.ApplyConfiguration(new TaskPriorityConfiguration());
        }
    }
}
