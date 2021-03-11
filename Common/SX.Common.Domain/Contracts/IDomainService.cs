using System;

namespace SX.Common.Domain.Contracts
{
    public interface IDomainService : IDisposable
    { }

    public interface IDomainService<TUnitOfWork> : IDomainService
        where TUnitOfWork : IDomainUnitOfWork
    {
        TUnitOfWork UnitOfWork { get; }
    }
}
