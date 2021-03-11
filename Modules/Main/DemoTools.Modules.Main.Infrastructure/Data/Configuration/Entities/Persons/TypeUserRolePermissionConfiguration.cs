using DemoTools.Modules.Main.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Persons
{
    public class TypeUserRolePermissionConfiguration : IEntityTypeConfiguration<TypeUserRolePermission>
    {
        public void Configure(EntityTypeBuilder<TypeUserRolePermission> builder)
        {
            builder.ToTable("TypesUserRolePermission").HasKey(t => new { t.UserRoleID, t.UserPermissionID });

            builder.HasOne(t => t.Role)
                .WithMany(t => t.RolePermissions)
                .HasForeignKey(t => t.UserRoleID);

            builder.HasOne(t => t.Permission)
                .WithMany()
                .HasForeignKey(t => t.UserPermissionID);
        }
    }
}
