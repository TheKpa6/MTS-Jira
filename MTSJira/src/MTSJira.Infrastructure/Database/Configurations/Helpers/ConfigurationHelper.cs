using MTSJira.Domain.Entities;

namespace MTSJira.Infrastructure.Database.Configurations.Helpers
{
    public static class ConfigurationHelper
    {
        public static IEnumerable<EnumEntity<TEnum>> GetEnumData<TEnum>()
                where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(x => new EnumEntity<TEnum>(x));
        }
    }
}
