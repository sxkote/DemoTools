using DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities;
using DemoTools.Modules.Main.Infrastructure.Data.Configuration.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using SX.Common.Infrastructure.Data;

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
            modelBuilder.ApplyConfiguration(new AuthTokenConfiguration());

            modelBuilder.ApplyConfiguration(new TypeUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new TypeUserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new TypeUserRolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());


            modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
            modelBuilder.ApplyConfiguration(new TodoListConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
