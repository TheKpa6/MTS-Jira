using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTSJira.Domain.Entities;
using MTSJira.Domain.Entities.Enums;
using MTSJira.Infrastructure.Database.Configurations.Helpers;

namespace MTSJira.Infrastructure.Database.Configurations
{
    public class TaskPriorityConfiguration : IEntityTypeConfiguration<EnumEntity<TaskPriority>>
    {
        public void Configure(EntityTypeBuilder<EnumEntity<TaskPriority>> builder)
        {
            builder.ToTable("task_priorities");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").IsRequired();
            builder.Property(t => t.Name).HasColumnName("name").IsRequired();

            builder.HasData(ConfigurationHelper.GetEnumData<TaskPriority>());
        }
    }
}
