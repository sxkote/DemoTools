using DemoTools.Modules.Main.Domain.Entities.Persons;
using SX.Common.Domain.Contracts;

namespace DemoTools.Modules.Main.Domain.Contracts.Repositories
{
    public interface ITypeUserRoleRepository : IDomainRepositoryGuid<TypeUserRole>
    {
        TypeUserRole Get(string name);
    }
}
