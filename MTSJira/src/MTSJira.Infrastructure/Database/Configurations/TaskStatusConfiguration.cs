using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTSJira.Domain.Entities;
using MTSJira.Infrastructure.Database.Configurations.Helpers;

namespace MTSJira.Infrastructure.Database.Configurations
{
    public class TaskStatusConfiguration : IEntityTypeConfiguration<EnumEntity<Domain.Entities.Enums.TaskStatus>>
    {
        public void Configure(EntityTypeBuilder<EnumEntity<Domain.Entities.Enums.TaskStatus>> builder)
        {
            builder.ToTable("task_statuses");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").IsRequired();
            builder.Property(t => t.Name).HasColumnName("name").IsRequired();

            builder.HasData(ConfigurationHelper.GetEnumData<Domain.Entities.Enums.TaskStatus>());
        }
    }
}
