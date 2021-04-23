using DemoTools.Records.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Records.Infrastructure.Data.Configuration
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {

            builder.ToTable("TodoList").HasKey(t => t.ID);

            builder.Property(t => t.ID)
                .HasColumnName("TodoListID");

            builder.Property(t => t.Title).HasStringType();

            builder.HasMany(t => t.Items)
                .WithOne()
                .HasForeignKey(t => t.TodoListID);
        }
    }
}
