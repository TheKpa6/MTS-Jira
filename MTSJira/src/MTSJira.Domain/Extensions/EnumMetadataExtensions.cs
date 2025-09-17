using MTSJira.Domain.Attributes;

namespace MTSJira.Domain.Extensions
{
    public static class EnumMetadataExtensions
    {
        private static object? GetMetadata(Enum @enum)
        {
            var type = @enum.GetType();
            var info = type.GetMember(@enum.ToString());
            if ((info != null) && (info.Length > 0))
            {
                object[] attrs = info[0].GetCustomAttributes(typeof(EnumMetadataAttribute), false);
                if ((attrs != null) && (attrs.Length > 0))
                {
                    return attrs[0];
                }
            }

            return null;
        }

        public static string Name(this Enum enumValue)
        {
            object? metadata = GetMetadata(enumValue);
            return (metadata != null) ? ((EnumMetadataAttribute)metadata).Name : enumValue.ToString();
        }
    }
}
