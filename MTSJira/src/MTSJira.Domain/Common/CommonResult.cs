namespace MTSJira.Domain.Common
{
    public abstract class CommonResult<TErrorCode, TData> : CommonResultNoData<TErrorCode>
     where TErrorCode : Enum
    {
        public TData Data { get; set; }

        public CommonResult()
        {
        }

        public CommonResult(Exception ex, string? message = null)
            : base(ex, message)
        {
        }

        public bool Failed(out TData data)
        {
            data = Data;
            return HasError;
        }
    }
}
