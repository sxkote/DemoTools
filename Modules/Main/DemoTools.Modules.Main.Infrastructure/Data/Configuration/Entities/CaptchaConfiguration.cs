using DemoTools.Modules.Main.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SX.Common.Infrastructure;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities
{
    public class CaptchaConfiguration : IEntityTypeConfiguration<Captcha>
    {
        public void Configure(EntityTypeBuilder<Captcha> builder)
        {

            builder.ToTable("Captcha").HasKey(t => t.ID); 

            builder.Property(t => t.ID)
                .HasColumnName("ID");

            builder.Property(t => t.Text).HasStringType();
        }
    }
}
