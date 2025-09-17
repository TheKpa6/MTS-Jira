using System.ComponentModel.DataAnnotations;

namespace MTSJira.Api.Models.Task
{
    /// <summary>
    /// Модель данных для обновления статуса задачи
    /// </summary>
    public class UpdateTaskStatusRequest
    {
        /// <summary>
        /// Статус задачи
        /// </summary>
        [Required]
        public Enums.TaskStatus TaskStatus { get; set; }
    }
}
