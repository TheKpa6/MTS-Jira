namespace MTSJira.Api.Models.Task
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public Enums.TaskStatus Status { get; set; }
        public Enums.TaskPriority Priority { get; set; }
        public string? Assignee { get; set; }
        public int? ParentTaskId { get; set; }

        public List<RelatedTask> RelatedTasks { get; set; } = new();
        public List<RelatedToTasks> RelatedToTasks { get; set; } = new();
    }
}
