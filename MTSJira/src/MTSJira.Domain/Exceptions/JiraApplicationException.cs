using MTSJira.Domain.Common.Enums;

namespace MTSJira.Domain.Exceptions
{
    public sealed class JiraApplicationException(string message, CommonErrorCode errorCode) : Exception(message)
    {
        public CommonErrorCode ErrorCode { get; } = errorCode;
    }
}
