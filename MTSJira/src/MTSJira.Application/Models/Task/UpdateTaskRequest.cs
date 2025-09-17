using MTSJira.Domain.Entities.Enums;

namespace MTSJira.Application.Models.Task
{
    public class UpdateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public Domain.Entities.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string? Author { get; set; }
        public string? Assignee { get; set; }
        public int? ParentTaskId { get; set; }

        public List<RelatedTaskDto> RelatedTasks { get; set; } = new();
        public List<RelatedToTasksDto> RelatedToTasks { get; set; } = new();
    }
}
