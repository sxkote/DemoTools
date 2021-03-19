using System.Collections.Generic;

namespace SX.Common.Shared.Contracts
{
    public interface IRepository
    { }

    public interface IRepository<T> : IRepository
        where T : class
    {
        IEnumerable<T> GetAll();

        void Add(IEnumerable<T> entities);

        T Add(T entity);

        T Update(T entity);

        void Delete(T entity);

        void Refresh(T entity);

        void Detach(T entity);

    }
}
