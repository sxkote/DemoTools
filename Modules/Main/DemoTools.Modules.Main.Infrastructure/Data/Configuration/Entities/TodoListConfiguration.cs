﻿using DemoTools.Modules.Main.Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {

            builder.ToTable("TodoList").HasKey(t => t.ID); 

            builder.Property(t => t.ID)
                .HasColumnName("TodoListID");

            builder.HasMany(t => t.Items)
                .WithOne()
                .HasForeignKey("TodoListID");
        }
    }
}
