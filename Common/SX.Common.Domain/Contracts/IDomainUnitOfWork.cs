using SX.Common.Domain.Interfaces;
using System;

namespace SX.Common.Domain.Contracts
{
    public interface IDomainUnitOfWork : IDisposable
    {
        void SaveChanges();


        TEntity GetEntity<TEntity>(object key) where TEntity : class, IEntity;
        void AddEntity<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void UpdateEntity<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void DeleteEntity<TEntity>(TEntity entity) where TEntity : class, IEntity;
    }
}
