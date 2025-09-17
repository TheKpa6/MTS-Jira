using System.ComponentModel.DataAnnotations;

namespace MTSJira.Api.Models.Task
{
    /// <summary>
    /// Модель для создания новой задачи
    /// </summary>
    public class CreateTaskRequest
    {
        /// <summary>
        /// Заголовок задачи
        /// </summary>
        [Required]
        [StringLength(1, MinimumLength = 50)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Статус задачи
        /// </summary>
        [Required]
        public Enums.TaskStatus Status { get; set; }

        /// <summary>
        /// Приоритет задачи
        /// </summary>
        [Required]
        public Enums.TaskPriority Priority { get; set; }

        /// <summary>
        /// Исполняющий
        /// </summary>
        public string? Assignee { get; set; }

        /// <summary>
        /// Идентификатор родительской задачи
        /// </summary>
        public int? ParentTaskId { get; set; }

        /// <summary>
        /// Исходящие связи задачи
        /// </summary>
        public List<RelatedTask> RelatedTasks { get; set; } = new();

        /// <summary>
        /// Входящие связи задачи
        /// </summary>
        public List<RelatedToTasks> RelatedToTasks { get; set; } = new();
    }
}
