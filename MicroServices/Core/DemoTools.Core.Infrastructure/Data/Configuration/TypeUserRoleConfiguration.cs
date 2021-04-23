using DemoTools.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Core.Infrastructure.Data.Configuration
{
    public class TypeUserRoleConfiguration : IEntityTypeConfiguration<TypeUserRole>
    {
        public void Configure(EntityTypeBuilder<TypeUserRole> builder)
        {

            builder.ToTable("TypesUserRole").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("UserRoleID");

            builder.Property(t => t.Name).HasStringType();
            builder.Property(t => t.Title).HasStringType();

        }
    }
}
