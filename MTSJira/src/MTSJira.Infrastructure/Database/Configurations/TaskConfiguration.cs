using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTSJira.Domain.Entities;
using MTSJira.Domain.Entities.Enums;

namespace MTSJira.Infrastructure.Database.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskData>
    {
        public void Configure(EntityTypeBuilder<TaskData> builder)
        {
            builder.ToTable("tasks").HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(t => t.Status).HasColumnName("status_id").IsRequired();
            builder.Property(t => t.Assignee).HasColumnName("assignee").HasMaxLength(100);
            builder.Property(t => t.Priority).HasColumnName("priority_id").IsRequired();
            builder.Property(t => t.Author).HasColumnName("author").HasMaxLength(100);
            builder.Property(t => t.ParentTaskId).HasColumnName("parent_task_id");
            builder.Property(t => t.Title).HasColumnName("title").HasMaxLength(100).IsRequired();

            builder.HasOne(t => t.ParentTask)
                   .WithMany(t => t.Subtasks)
                   .HasForeignKey(t => t.ParentTaskId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(EnumEntity<Domain.Entities.Enums.TaskStatus>)).WithMany().HasForeignKey(nameof(TaskData.Status));
            builder.HasOne(typeof(EnumEntity<TaskPriority>)).WithMany().HasForeignKey(nameof(TaskData.Priority));
        }
    }
}
