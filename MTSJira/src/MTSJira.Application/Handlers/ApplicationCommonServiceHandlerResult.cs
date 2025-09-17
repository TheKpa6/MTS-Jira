using MTSJira.Domain.Common;
using MTSJira.Domain.Common.Enums;

namespace MTSJira.Application.Handlers
{
    public class ApplicationCommonServiceHandlerResult<TData> : CommonResult<CommonErrorCode, TData>
    {
        public override bool HasError => ErrorCode != CommonErrorCode.None;

        public static ApplicationCommonServiceHandlerResult<TData> CreateSuccess(TData data, string? message = null)
        {
            return new ApplicationCommonServiceHandlerResult<TData>
            {
                Data = data,
                Message = message,
                ErrorCode = CommonErrorCode.None
            };
        }

        public static ApplicationCommonServiceHandlerResult<TData> CreateException(Exception exception, string? message = null)
        {
            return new ApplicationCommonServiceHandlerResult<TData>
            {
                Ex = exception,
                Message = message ?? exception.Message,
                ErrorCode = CommonErrorCode.Exception
            };
        }

        public static ApplicationCommonServiceHandlerResult<TData> CreateError(CommonErrorCode errorCode, string? message = null)
        {
            return new ApplicationCommonServiceHandlerResult<TData>
            {
                ErrorCode = errorCode,
                Message = message,
            };
        }
    }
}
