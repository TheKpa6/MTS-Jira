namespace MTSJira.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumMetadataAttribute : Attribute
    {
        public EnumMetadataAttribute()
        {
            Name = string.Empty;
        }

        public EnumMetadataAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
