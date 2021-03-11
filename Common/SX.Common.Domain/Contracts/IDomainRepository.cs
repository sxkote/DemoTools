using SX.Common.Domain.Entities;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace SX.Common.Domain.Contracts
{
    public interface IDomainRepository<TEntity, TKey> : IRepository<TEntity>
      where TEntity : Entity<TKey>
    {
        TEntity Get(TKey id);

        TEntity Modify(TEntity entity);

        void Delete(TKey id);

        TEntity GetTracking(TKey id, bool multiple = false);
        IEnumerable<TEntity> GetTracking(IEnumerable<TKey> ids, bool multiple = false);
    }

    public interface IDomainRepository32<TEntity> : IDomainRepository<TEntity, int>
        where TEntity : Entity32
    { }

    public interface IDomainRepository64<TEntity> : IDomainRepository<TEntity, long>
        where TEntity : Entity64
    { }

    public interface IDomainRepositoryGuid<TEntity> : IDomainRepository<TEntity, Guid>
        where TEntity : EntityGuid
    { }
}
