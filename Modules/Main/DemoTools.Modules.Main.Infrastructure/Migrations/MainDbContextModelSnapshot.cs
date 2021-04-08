﻿// <auto-generated />
using System;
using DemoTools.Modules.Main.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DemoTools.Modules.Main.Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Activity", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Expire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("PersonID")
                        .HasColumnType("uuid");

                    b.Property<string>("Pin")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .IsFixedLength(true);

                    b.Property<string>("Type")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.AuthToken", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("Expire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.ToTable("AuthToken");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Captcha", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ID");

                    b.Property<DateTimeOffset>("Expire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Text")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID");

                    b.ToTable("Captcha");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.Person", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("PersonID");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Cellular")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.Property<byte>("Gender")
                        .HasColumnType("smallint");

                    b.Property<Guid>("SubscriptionID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            ID = new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"),
                            Cellular = "+7-909-900-7011",
                            Email = "ivan@litskevich.ru",
                            Gender = (byte)1,
                            SubscriptionID = new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55")
                        });
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserPermission", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("UserPermissionID");

                    b.Property<string>("Category")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID");

                    b.ToTable("TypesUserPermission");

                    b.HasData(
                        new
                        {
                            ID = new Guid("bf87080f-e529-4520-885f-29f492fe63fe"),
                            Category = "001",
                            Name = "Login",
                            Title = "Login"
                        });
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserRole", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("UserRoleID");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID");

                    b.ToTable("TypesUserRole");

                    b.HasData(
                        new
                        {
                            ID = new Guid("45e7071e-85c2-4be2-8594-505aae4bf0d1"),
                            Name = "Administrator",
                            Title = "Administrator"
                        },
                        new
                        {
                            ID = new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f"),
                            Name = "User",
                            Title = "User"
                        });
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserRolePermission", b =>
                {
                    b.Property<Guid>("UserRoleID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserPermissionID")
                        .HasColumnType("uuid");

                    b.HasKey("UserRoleID", "UserPermissionID");

                    b.HasIndex("UserPermissionID");

                    b.ToTable("TypesUserRolePermission");

                    b.HasData(
                        new
                        {
                            UserRoleID = new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f"),
                            UserPermissionID = new Guid("bf87080f-e529-4520-885f-29f492fe63fe")
                        });
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.User", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uuid")
                        .HasColumnName("UserID");

                    b.Property<string>("Login")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"),
                            Login = "guest",
                            Password = "123456"
                        });
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.UserRole", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserRoleID")
                        .HasColumnType("uuid");

                    b.HasKey("UserID", "UserRoleID");

                    b.HasIndex("UserRoleID");

                    b.ToTable("UsersRoles");

                    b.HasData(
                        new
                        {
                            UserID = new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"),
                            UserRoleID = new Guid("45e7071e-85c2-4be2-8594-505aae4bf0d1")
                        },
                        new
                        {
                            UserID = new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"),
                            UserRoleID = new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f")
                        });
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Subscription", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("SubscriptionID");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .IsFixedLength(true);

                    b.HasKey("ID");

                    b.ToTable("Subscription");

                    b.HasData(
                        new
                        {
                            ID = new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55"),
                            Title = "Demo-Subscription"
                        });
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Todo.TodoItem", b =>
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
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Todo.TodoList", b =>
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
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.Person", b =>
                {
                    b.OwnsOne("SX.Common.Shared.Models.PersonFullName", "Name", b1 =>
                        {
                            b1.Property<Guid>("PersonID")
                                .HasColumnType("uuid");

                            b1.Property<string>("First")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("NameFirst")
                                .IsFixedLength(true);

                            b1.Property<string>("Last")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("NameLast")
                                .IsFixedLength(true);

                            b1.Property<string>("Second")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("NameSecond")
                                .IsFixedLength(true);

                            b1.HasKey("PersonID");

                            b1.ToTable("Persons");

                            b1.WithOwner()
                                .HasForeignKey("PersonID");

                            b1.HasData(
                                new
                                {
                                    PersonID = new Guid("37846734-172e-4149-8cec-6f43d1eb3f60"),
                                    First = "John",
                                    Last = "Doe",
                                    Second = ""
                                });
                        });

                    b.Navigation("Name");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserRolePermission", b =>
                {
                    b.HasOne("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserPermission", "Permission")
                        .WithMany()
                        .HasForeignKey("UserPermissionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserRole", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("UserRoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.User", b =>
                {
                    b.HasOne("DemoTools.Modules.Main.Domain.Entities.Persons.Person", null)
                        .WithOne("User")
                        .HasForeignKey("DemoTools.Modules.Main.Domain.Entities.Persons.User", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.UserRole", b =>
                {
                    b.HasOne("DemoTools.Modules.Main.Domain.Entities.Persons.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserRole", "Role")
                        .WithMany()
                        .HasForeignKey("UserRoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Todo.TodoItem", b =>
                {
                    b.HasOne("DemoTools.Modules.Main.Domain.Entities.Todo.TodoList", null)
                        .WithMany("Items")
                        .HasForeignKey("TodoListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.Person", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.TypeUserRole", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Persons.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("DemoTools.Modules.Main.Domain.Entities.Todo.TodoList", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
