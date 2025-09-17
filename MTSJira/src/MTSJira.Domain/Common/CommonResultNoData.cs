namespace MTSJira.Domain.Common
{
    public abstract class CommonResultNoData<TErrorCode>
    where TErrorCode : Enum
    {
        public TErrorCode ErrorCode { get; set; }

        public abstract bool HasError { get; }

        public string? Message { get; set; }

        public Exception Ex { get; set; }

        protected CommonResultNoData()
        {
        }

        protected CommonResultNoData(Exception ex, string? message = null)
        {
            Ex = ex;
            Message = message;
        }

        protected static T CreateException<T>(Exception ex, TErrorCode exceptionCode, string? message = null)
            where T : CommonResultNoData<TErrorCode>, new()
        {
            string errMessage = message ?? ex.Message;
            return new T
            {
                Ex = ex,
                ErrorCode = exceptionCode,
                Message = errMessage,
            };
        }
    }
}
