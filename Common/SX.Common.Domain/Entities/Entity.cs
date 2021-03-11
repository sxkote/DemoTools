using SX.Common.Domain.Enums;
using SX.Common.Domain.Interfaces;
using System;

namespace SX.Common.Domain.Entities
{
    public abstract class Entity<T> : IdentifiableEntity<T>, IEntity, IDbState
    { 
        protected DbState _dbState = DbState.None;

        DbState IDbState.DbState { get { return _dbState; } }

        void IDbState.ChangeDbState(DbState state)
        { _dbState = state; }

        void IDbState.DeleteFromDb()
        { _dbState = DbState.Deleted; }
    }

    public abstract class Entity32 : Entity<int> { }
    public abstract class Entity64 : Entity<long> { }
    public abstract class EntityGuid : Entity<Guid> { }
}
