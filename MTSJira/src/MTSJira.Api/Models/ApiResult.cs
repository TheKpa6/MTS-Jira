namespace MTSJira.Api.Models
{
    public class ApiResult<TData> : ApiResultNoData
    {
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
