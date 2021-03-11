using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using SX.Common.Domain.Contracts;

namespace DemoTools.Modules.Main.Domain.Contracts
{
    public interface IMainUnitOfWork : IDomainUnitOfWork
    {
        ITodoListRepository TodoListRepository { get; }
    }
}
