using MTSJira.Domain.Entities.Enums;

namespace MTSJira.Domain.Entities
{
    public class TaskData
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string? Author { get; set; }
        public string? Assignee { get; set; }
        public int? ParentTaskId { get; set; }

        public virtual TaskData? ParentTask { get; set; }
        public virtual ICollection<TaskData> Subtasks { get; set; } = new List<TaskData>();
        public virtual ICollection<TaskRelationshipData> RelatedTasks { get; set; } = new List<TaskRelationshipData>();
        public virtual ICollection<TaskRelationshipData> RelatedToTasks { get; set; } = new List<TaskRelationshipData>();
    }
}
