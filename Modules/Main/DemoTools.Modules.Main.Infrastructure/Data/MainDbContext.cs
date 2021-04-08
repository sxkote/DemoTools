using DemoTools.Modules.Main.Domain.Entities;
using DemoTools.Modules.Main.Domain.Entities.Persons;
using DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities;
using DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Persons;
using DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Todo;
using Microsoft.EntityFrameworkCore;
using SX.Common.Infrastructure.Data;
using System;

namespace DemoTools.Modules.Main.Infrastructure.Data
{
    public class MainDbContext : DomainDbContext
    {
        public MainDbContext() : base() { }

        public MainDbContext(DbContextOptions<MainDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new AuthTokenConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityConfiguration());
            modelBuilder.ApplyConfiguration(new CaptchaConfiguration());

            modelBuilder.ApplyConfiguration(new TypeUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new TypeUserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new TypeUserRolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());


            modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
            modelBuilder.ApplyConfiguration(new TodoListConfiguration());

            this.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected void Seed(ModelBuilder modelBuilder)
        {
            Guid subscriptionID = new Guid("831b3626-6359-4f56-b5c2-81d86efcdc55");
            modelBuilder.Entity<Subscription>().HasData(
                new
                {
                    ID = subscriptionID,
                    Title = "Demo-Subscription"
                });

            // Permissions
            Guid permLoginID = new Guid("bf87080f-e529-4520-885f-29f492fe63fe");
            modelBuilder.Entity<TypeUserPermission>().HasData(
                new
                {
                    ID = permLoginID,
                    Name = "Login",
                    Title = "Login",
                    Category = "001"
                });

            // Roles
            Guid roleAdminID = new Guid("45e7071e-85c2-4be2-8594-505aae4bf0d1");
            Guid roleUserID = new Guid("6b823d5e-949f-4be5-926b-8dde77112d0f");
            modelBuilder.Entity<TypeUserRole>().HasData(
                new
                {
                    ID = roleAdminID,
                    Name = "Administrator",
                    Title = "Administrator"
                },
                new
                {
                    ID = roleUserID,
                    Name = "User",
                    Title = "User"
                });

            // Roles Permissions
            modelBuilder.Entity<TypeUserRolePermission>().HasData(new
                {
                    UserRoleID = roleUserID,
                    UserPermissionID = permLoginID
                });


            // Person
            Guid personID = new System.Guid("37846734-172e-4149-8cec-6f43d1eb3f60");
            modelBuilder.Entity<Person>().HasData(
                new
                {
                    ID = personID,
                    SubscriptionID = subscriptionID,
                    Cellular = "+7-909-900-7011",
                    Email = "ivan@litskevich.ru",
                    BirthDate = (System.DateTime?)null,
                    Gender = SX.Common.Shared.Enums.Gender.Male
                });
            modelBuilder.Entity<Person>().OwnsOne(t => t.Name).HasData(
                new
                {
                    PersonID = personID,
                    First = "John",
                    Last = "Doe",
                    Second = ""
                });
            modelBuilder.Entity<User>().HasData(
                new
                {
                    ID = personID,
                    Login = "guest",
                    Password = "123456"
                });
            modelBuilder.Entity<UserRole>().HasData(
                new
                {
                    UserID = personID,
                    UserRoleID = roleAdminID
                });
            modelBuilder.Entity<UserRole>().HasData(
              new
              {
                  UserID = personID,
                  UserRoleID = roleUserID
              });
        }
    }
}
