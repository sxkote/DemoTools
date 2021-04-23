using DemoTools.Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTools.Authorization.Infrastructure.Data.Configuration
{
    public class TypeUserPermissionConfiguration : IEntityTypeConfiguration<TypeUserPermission>
    {
        public void Configure(EntityTypeBuilder<TypeUserPermission> builder)
        {

            builder.ToTable("TypesUserPermission").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("UserPermissionID");

            builder.Property(t => t.Name).HasStringType();
            builder.Property(t => t.Title).HasStringType();
            builder.Property(t => t.Category).HasStringType();
        }
    }
}
