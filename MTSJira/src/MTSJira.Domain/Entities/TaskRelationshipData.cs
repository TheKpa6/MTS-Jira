namespace MTSJira.Domain.Entities
{
    public class TaskRelationshipData
    {
        public int Id { get; set; }

        public int SourceTaskId { get; set; }
        public virtual TaskData SourceTask { get; set; } = null!;

        public int RelatedTaskId { get; set; }
        public virtual TaskData RelatedTask { get; set; } = null!;
    }
}
