using DemoTools.Records.Domain.Entities;
using DemoTools.Records.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace DemoTools.Records.Infrastructure.Data
{
    public class RecordsDbContext : DbContext
    {
        public RecordsDbContext()
            : base() { }

        public RecordsDbContext(DbContextOptions<RecordsDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
            modelBuilder.ApplyConfiguration(new TodoListConfiguration());

            this.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected void Seed(ModelBuilder modelBuilder)
        {
            Guid subscriptionID = new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55");


            Guid todoList1 = new Guid("207df05f-ca31-40ad-8895-0800009398eb");
            Guid todoList2 = new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0");
            modelBuilder.Entity<TodoList>().HasData(
                new
                {
                    ID = todoList1,
                    SubscriptionID = subscriptionID,
                    Title = "New List - 1"
                },
                new
                {
                    ID = todoList2,
                    SubscriptionID = subscriptionID,
                    Title = "New List - 2"
                });

            Guid todoItem11 = new Guid("5dea32f8-b10f-4ddf-a49b-8b65f77e004d");
            Guid todoItem21 = new Guid("c7eb2bf8-cbe2-4425-b136-f4c56bdb69d9");
            Guid todoItem22 = new Guid("47f66d75-354b-4a1c-9738-45daf0aed4b5");
            modelBuilder.Entity<TodoItem>().HasData(
                new
                {
                    ID = todoItem11,
                    TodoListID = todoList1,
                    Title = "TodoItem - 1.1",
                    IsDone = false
                },
                new
                {
                    ID = todoItem21,
                    TodoListID = todoList2,
                    Title = "TodoItem - 2.1",
                    IsDone = false
                },
                new
                {
                    ID = todoItem22,
                    TodoListID = todoList2,
                    Title = "TodoItem - 2.2",
                    IsDone = true
                });
        }
    }
}
