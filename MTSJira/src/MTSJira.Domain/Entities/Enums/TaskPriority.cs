using MTSJira.Domain.Attributes;

namespace MTSJira.Domain.Entities.Enums
{
    public enum TaskPriority
    {
        [EnumMetadata("Low")]
        Low,

        [EnumMetadata("Medium")]
        Medium,

        [EnumMetadata("High")]
        High,
    }
}
