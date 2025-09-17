namespace MTSJira.Api.Models
{
    public class ApiResultNoData
    {
        public string? Message { get; set; }

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
