using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Records.Infrastructure
{
    public static class Extensions
    {
        public const string DBTYPE_STRING = "character varying";
        public const string DBTYPE_TEXT = "text";

        public static PropertyBuilder<string> HasStringType(this PropertyBuilder<string> propertyBuilder, int maxLength = 100)
        {
            return propertyBuilder
                .HasColumnType($"{DBTYPE_STRING}({maxLength})")
                .HasMaxLength(maxLength)
                .IsFixedLength(true);
        }

        public static PropertyBuilder<string> HasTextType(this PropertyBuilder<string> propertyBuilder)
        {
            return propertyBuilder.HasColumnType(DBTYPE_TEXT);
        }
    }
}
