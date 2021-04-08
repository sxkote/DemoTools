using DemoTools.Modules.Main.Domain.Entities.Todo;
using SX.Common.Domain.Contracts;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Domain.Contracts.Repositories
{
    public interface ITodoListRepository : IDomainRepositoryGuid<TodoList>
    {
        IEnumerable<TodoList> GetAll(Guid subscriptionID);
    }
}
