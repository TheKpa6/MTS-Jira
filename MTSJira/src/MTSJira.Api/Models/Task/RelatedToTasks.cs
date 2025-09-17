namespace MTSJira.Api.Models.Task
{
    /// <summary>
    /// Модель данных для входящих связий задачи
    /// </summary>
    public class RelatedToTasks
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public int SourceTaskId { get; set; }
    }
}
