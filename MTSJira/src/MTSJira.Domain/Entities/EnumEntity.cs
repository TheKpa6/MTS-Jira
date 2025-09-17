using MTSJira.Domain.Extensions;

namespace MTSJira.Domain.Entities
{
    public class EnumEntity<TEnum>
        where TEnum : Enum
    {
        public EnumEntity()
        {
        }

        public EnumEntity(TEnum value)
        {
            Id = value;
            Name = value.Name();
        }

        public TEnum Id { get; set; }

        public string Name { get; set; }
    }
}
