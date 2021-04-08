using DemoTools.Modules.Main.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SX.Common.Infrastructure;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Persons
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
