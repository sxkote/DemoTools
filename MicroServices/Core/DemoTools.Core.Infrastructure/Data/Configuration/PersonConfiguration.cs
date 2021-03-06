using DemoTools.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace DemoTools.Core.Infrastructure.Data.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {

            builder.ToTable("Persons").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("PersonID");

            builder.Property(t => t.Email).HasStringType(100);
            builder.Property(t => t.Cellular).HasStringType(100);

            builder.OwnsOne(t => t.Name, f =>
            {
                f.Property(p => p.First).HasColumnName("NameFirst").HasStringType();
                f.Property(p => p.Last).HasColumnName("NameLast").HasStringType();
                f.Property(p => p.Second).HasColumnName("NameSecond").HasStringType();
            });


            builder.HasOne(t => t.User)
                .WithOne()
                .HasForeignKey<User>(t => t.ID);
        }
    }
}
