namespace MTSJira.Api.Models.Task
{
    public class UpdateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public Enums.TaskStatus Status { get; set; }
        public Enums.TaskPriority Priority { get; set; }
        public string? Author { get; set; }
        public string? Assignee { get; set; }
        public int? ParentTaskId { get; set; }

        public List<RelatedTask> RelatedTasks { get; set; } = new();
        public List<RelatedToTasks> RelatedToTasks { get; set; } = new();
    }

    public class RelatedTask
    {
        public int RelatedTaskId { get; set; }
    }

    public class RelatedToTasks
    {
        public int SourceTaskId { get; set; }
    }
}
