using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTSJira.Domain.Entities;
using System.Reflection.Emit;

namespace MTSJira.Infrastructure.Database.Configurations
{
    public class TaskRelationshipConfiguration : IEntityTypeConfiguration<TaskRelationshipData>
    {
        public void Configure(EntityTypeBuilder<TaskRelationshipData> builder)
        {
            builder.ToTable("tasks_relationships").HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(t => t.SourceTaskId).HasColumnName("source_task_id").IsRequired();
            builder.Property(t => t.RelatedTaskId).HasColumnName("related_task_id").IsRequired();

            builder.HasOne(tr => tr.SourceTask)
                   .WithMany(t => t.RelatedTasks)
                   .HasForeignKey(tr => tr.SourceTaskId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.RelatedTask)
                   .WithMany(t => t.RelatedToTasks)
                   .HasForeignKey(tr => tr.RelatedTaskId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
