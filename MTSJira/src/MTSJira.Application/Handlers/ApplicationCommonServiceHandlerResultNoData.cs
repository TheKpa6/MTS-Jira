using MTSJira.Domain.Common;
using MTSJira.Domain.Common.Enums;

namespace MTSJira.Application.Handlers
{
    public class ApplicationCommonServiceHandlerResultNoData : CommonResultNoData<CommonErrorCode>
    {
        public override bool HasError => ErrorCode != CommonErrorCode.None;

        public static ApplicationCommonServiceHandlerResultNoData CreateSuccess(string? message = null)
        {
            return new ApplicationCommonServiceHandlerResultNoData
            {
                Message = message,
                ErrorCode = CommonErrorCode.None
            };
        }

        public static ApplicationCommonServiceHandlerResultNoData CreateException(Exception exception, string? message = null)
        {
            return new ApplicationCommonServiceHandlerResultNoData
            {
                Ex = exception,
                Message = message ?? exception.Message,
                ErrorCode = CommonErrorCode.Exception
            };
        }

        public static ApplicationCommonServiceHandlerResultNoData CreateError(CommonErrorCode errorCode, string? message = null)
        {
            return new ApplicationCommonServiceHandlerResultNoData
            {
                ErrorCode = errorCode,
                Message = message
            };
        }
    }
}
