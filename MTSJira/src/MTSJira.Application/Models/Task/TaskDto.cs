using MTSJira.Domain.Enums;

namespace MTSJira.Application.Models.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Domain.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string? Author { get; set; }
        public string? Assignee { get; set; }
        public int? ParentTaskId { get; set; }
        public TaskDto? ParentTask { get; set; }
        public ICollection<TaskDto> Subtasks { get; set; } = new List<TaskDto>();
    }
}
