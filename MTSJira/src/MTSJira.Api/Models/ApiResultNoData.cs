namespace MTSJira.Api.Models
{
    /// <summary>
    /// Модель результата работы апи метода без данных
    /// </summary>
    public class ApiResultNoData
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Стек трейс ошибки
        /// </summary>
        public string? StackTrace { get; set; }

        public ApiResultNoData()
        {
        }

        public ApiResultNoData(string? message, string? stackTrace = null)
        {
            StackTrace = stackTrace;
            Message = message;
        }
    }
}
