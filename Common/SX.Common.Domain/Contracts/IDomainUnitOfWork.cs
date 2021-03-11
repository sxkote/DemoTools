using System;

namespace SX.Common.Domain.Contracts
{
    public interface IDomainUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
