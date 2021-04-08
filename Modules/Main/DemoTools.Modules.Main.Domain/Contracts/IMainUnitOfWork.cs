using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Entities;
using SX.Common.Domain.Contracts;
using System;

namespace DemoTools.Modules.Main.Domain.Contracts
{
    public interface IMainUnitOfWork : IDomainUnitOfWork
    {
        ITodoListRepository TodoListRepository { get; }
        ITypeUserRoleRepository TypeUserRoleRepository { get; }
        IPersonRepository PersonRepository { get; }
    }
}
