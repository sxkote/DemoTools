using DemoTools.Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Authorization.Infrastructure.Data.Configuration
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {

            builder.ToTable("Activity").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("ID");

            builder.Property(t => t.Type).HasStringType();
            builder.Property(t => t.Data).HasTextType();
            builder.Property(t => t.Pin).HasStringType(20);
        }
    }
}
