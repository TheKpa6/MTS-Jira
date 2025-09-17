using MTSJira.Application.Models.Task.Enums;

namespace MTSJira.Application.Models.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string? Author { get; set; }
        public string? Assignee { get; set; }
        public int? ParentTaskId { get; set; }
        public ICollection<int> SubtasksIds { get; set; } = new List<int>();
        public ICollection<TaskRelationshipDto> RelatedTasksIds { get; set; } = new List<TaskRelationshipDto>();
        public ICollection<TaskRelationshipDto> RelatedToTasksIds { get; set; } = new List<TaskRelationshipDto>();
    }

    public class TaskRelationshipDto
    {
        public int SourceTaskId { get; set; }

        public int RelatedTaskId { get; set; }
    }
}
