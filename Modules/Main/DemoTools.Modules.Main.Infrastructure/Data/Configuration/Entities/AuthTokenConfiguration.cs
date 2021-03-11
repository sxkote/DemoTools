using DemoTools.Modules.Main.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities
{
    public class AuthTokenConfiguration : IEntityTypeConfiguration<AuthToken>
    {
        public void Configure(EntityTypeBuilder<AuthToken> builder)
        {

            builder.ToTable("AuthToken").HasKey(t => t.ID); 

            builder.Property(t => t.ID)
                .HasColumnName("ID");
        }
    }
}
