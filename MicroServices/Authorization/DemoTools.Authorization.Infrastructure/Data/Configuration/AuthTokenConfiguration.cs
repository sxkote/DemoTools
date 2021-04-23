using DemoTools.Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Authorization.Infrastructure.Data.Configuration
{
    public class AuthTokenConfiguration : IEntityTypeConfiguration<AuthToken>
    {
        public void Configure(EntityTypeBuilder<AuthToken> builder)
        {
            builder.ToTable("AuthToken").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("ID");

            builder.Property(t => t.Data).HasTextType();
        }
    }
}
