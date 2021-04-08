using DemoTools.Modules.Main.Domain.Entities.Persons;
using SX.Common.Domain.Contracts;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Domain.Contracts.Repositories
{
    public interface IPersonRepository : IDomainRepositoryGuid<Person>
    {
        IEnumerable<Person> GetByLogin(string login);
    }
}
