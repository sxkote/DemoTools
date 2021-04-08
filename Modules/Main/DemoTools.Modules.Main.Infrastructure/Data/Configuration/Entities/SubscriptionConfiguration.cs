using DemoTools.Modules.Main.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SX.Common.Infrastructure;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities
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
