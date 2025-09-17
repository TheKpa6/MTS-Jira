using MTSJira.Domain.Entities.Enums;

namespace MTSJira.Application.Models.Task
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public Domain.Entities.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string? Assignee { get; set; }
        public int? ParentTaskId { get; set; }
    }
}
