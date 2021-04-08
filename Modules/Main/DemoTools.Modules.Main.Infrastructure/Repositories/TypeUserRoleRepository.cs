using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Entities.Persons;
using DemoTools.Modules.Main.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SX.Common.Infrastructure.Services;
using System.Linq;

namespace DemoTools.Modules.Main.Infrastructure.Repositories
{
    public class TypeUserRoleRepository : DomainRepositoryGuid<MainDbContext, TypeUserRole>, ITypeUserRoleRepository
    {
        public TypeUserRoleRepository(MainDbContext dbContext)
               : base(dbContext)
        { }

        protected override IQueryable<TypeUserRole> GetQuery(bool multiple, bool tracking = false)
        {
            var query = this.DbSet.AsQueryable();

            if (!multiple)
            {
                query = query.Include(t => t.RolePermissions)
                    .ThenInclude(rp => rp.Permission);
            }

            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        public TypeUserRole Get(string name)
        {
            return this.GetQuery(false, true)
                .FirstOrDefault(t => t.Name.ToLower() == name.ToLower());
        }
    }
}
