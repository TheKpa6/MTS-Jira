using MTSJira.Domain.Attributes;

namespace MTSJira.Domain.Entities.Enums
{
    public enum TaskStatus
    {
        [EnumMetadata("New")]
        New,

        [EnumMetadata("InProgress")]
        InProgress,

        [EnumMetadata("Done")]
        Done,
    }
}
