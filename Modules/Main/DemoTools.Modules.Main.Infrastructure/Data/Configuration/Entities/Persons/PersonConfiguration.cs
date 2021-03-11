using DemoTools.Modules.Main.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Persons
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {

            builder.ToTable("Persons").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("PersonID");

            builder.OwnsOne(t => t.Name, f =>
            {
                f.Property(p => p.First).HasColumnName("NameFirst");
                f.Property(p => p.Last).HasColumnName("NameLast");
                f.Property(p => p.Second).HasColumnName("NameSecond");
            });


            builder.HasOne(t => t.User)
                .WithOne()
                .HasForeignKey<User>(t => t.ID);
        }
    }
}
