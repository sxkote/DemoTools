using DemoTools.Modules.Main.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Persons
{
    public class TypeUserPermissionConfiguration : IEntityTypeConfiguration<TypeUserPermission>
    {
        public void Configure(EntityTypeBuilder<TypeUserPermission> builder)
        {

            builder.ToTable("TypesUserPermission").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("UserPermissionID");

        }
    }
}
