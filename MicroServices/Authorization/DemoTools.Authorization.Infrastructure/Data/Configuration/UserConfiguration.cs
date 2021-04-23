using DemoTools.Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Authorization.Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("UserID");

            builder.Property(t => t.Login).HasStringType();
            builder.Property(t => t.Password).HasStringType();
        }
    }
}
