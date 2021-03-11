using DemoTools.Modules.Main.Domain.Entities.Todos;
using SX.Common.Domain.Contracts;

namespace DemoTools.Modules.Main.Domain.Contracts.Repositories
{
    public interface ITodoListRepository : IDomainRepositoryGuid<TodoList>
    {
    }
}
