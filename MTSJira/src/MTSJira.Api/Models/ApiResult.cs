namespace MTSJira.Api.Models
{
    /// <summary>
    /// Модель результата работы апи метода с данными
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class ApiResult<TData> : ApiResultNoData
    {
        /// <summary>
        /// Данные
        /// </summary>
        public TData Data { get; set; }

        public ApiResult(TData data)
        {
            Data = data;
        }

        public ApiResult(string? message, string? stackTrace = null)
            : base(message, stackTrace)
        {
        }
    }
}
