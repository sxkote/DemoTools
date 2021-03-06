// <auto-generated />
using System;
using DemoTools.Records.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DemoTools.Records.Infrastructure.Migrations
{
    [DbContext(typeof(RecordsDbContext))]
    partial class RecordsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DemoTools.Records.Domain.Entities.TodoItem", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uuid")
                        .HasColumnName("TodoItemID");

                    b.Property<Guid>("TodoListID")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDone")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID", "TodoListID");

                    b.HasIndex("TodoListID");

                    b.ToTable("TodoItem");

                    b.HasData(
                        new
                        {
                            ID = new Guid("5dea32f8-b10f-4ddf-a49b-8b65f77e004d"),
                            TodoListID = new Guid("207df05f-ca31-40ad-8895-0800009398eb"),
                            IsDone = false,
                            Title = "TodoItem - 1.1"
                        },
                        new
                        {
                            ID = new Guid("c7eb2bf8-cbe2-4425-b136-f4c56bdb69d9"),
                            TodoListID = new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0"),
                            IsDone = false,
                            Title = "TodoItem - 2.1"
                        },
                        new
                        {
                            ID = new Guid("47f66d75-354b-4a1c-9738-45daf0aed4b5"),
                            TodoListID = new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0"),
                            IsDone = true,
                            Title = "TodoItem - 2.2"
                        });
                });

            modelBuilder.Entity("DemoTools.Records.Domain.Entities.TodoList", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("TodoListID");

                    b.Property<Guid>("SubscriptionID")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID");

                    b.ToTable("TodoList");

                    b.HasData(
                        new
                        {
                            ID = new Guid("207df05f-ca31-40ad-8895-0800009398eb"),
                            SubscriptionID = new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55"),
                            Title = "New List - 1"
                        },
                        new
                        {
                            ID = new Guid("27b9efe2-3f8d-4685-9ef9-a17deb4de2a0"),
                            SubscriptionID = new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55"),
                            Title = "New List - 2"
                        });
                });

            modelBuilder.Entity("DemoTools.Records.Domain.Entities.TodoItem", b =>
                {
                    b.HasOne("DemoTools.Records.Domain.Entities.TodoList", null)
                        .WithMany("Items")
                        .HasForeignKey("TodoListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DemoTools.Records.Domain.Entities.TodoList", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
