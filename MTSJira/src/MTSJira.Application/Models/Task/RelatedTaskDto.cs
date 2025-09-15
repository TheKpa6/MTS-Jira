namespace MTSJira.Application.Models.Task
{
    public class RelatedTaskDto
    {
        public int TaskId { get; set; }
        public string RelationshipType { get; set; } = string.Empty;
    }
}
