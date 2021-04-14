using DemoTools.Modules.Main.Domain.Entities.Todo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Todo
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
