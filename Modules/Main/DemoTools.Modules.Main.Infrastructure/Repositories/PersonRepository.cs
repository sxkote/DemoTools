using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Entities.Persons;
using DemoTools.Modules.Main.Domain.Entities.Todo;
using DemoTools.Modules.Main.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SX.Common.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Modules.Main.Infrastructure.Repositories
{
    public class PersonRepository : DomainRepositoryGuid<MainDbContext, Person>, IPersonRepository
    {
        public PersonRepository(MainDbContext dbContext)
               : base(dbContext)
        { }

        protected override IQueryable<Person> GetQuery(bool multiple, bool tracking = false)
        {
            var query = this.DbSet.AsQueryable();

            if (!multiple)
            {
                query = query.Include(t => t.User)
                    .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission);
            }
            else
            {
                query = query.Include(t => t.User);
            }

            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        public IEnumerable<Person> GetByLogin(string login)
        {
           return this.QueryAll
                .Where(t => t.User.Login.ToLower() == login.ToLower())
                .ToList();
        }
    }
}
