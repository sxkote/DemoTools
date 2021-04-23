using DemoTools.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Core.Infrastructure.Data.Configuration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {

            builder.ToTable("Subscription").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("SubscriptionID");

            builder.Property(t => t.Title).HasStringType();
        }
    }
}
