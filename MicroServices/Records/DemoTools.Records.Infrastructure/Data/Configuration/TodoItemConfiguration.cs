using DemoTools.Records.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Records.Infrastructure.Data.Configuration
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.ToTable("TodoItem").HasKey(t => new { t.ID, t.TodoListID });

            builder.Property(t => t.ID)
                .HasColumnName("TodoItemID");

            builder.Property(t => t.Title).HasStringType();

        }
    }
}
