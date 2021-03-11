using Microsoft.EntityFrameworkCore;
using SX.Common.Domain.Contracts;
using SX.Common.Domain.Interfaces;
using SX.Common.Infrastructure.Data;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace SX.Common.Infrastructure.Services
{
    public class DomainUnitOfWork<TDbContext> : IDomainUnitOfWork
                where TDbContext : DomainDbContext, new()
    {
        protected Dictionary<Type, IRepository> _repositories;
        //protected List<DbContextTransaction> _transactions;

        protected TDbContext _dbContext;

        protected TDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new TDbContext();

                return _dbContext;
            }
        }


        public DomainUnitOfWork()
        {
            _dbContext = new TDbContext();
            _repositories = new Dictionary<Type, IRepository>();
        }

        public DomainUnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, IRepository>();
        }


        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public virtual void Dispose()
        {
            _dbContext.Dispose();
        }

        public virtual T GetRepository<T>() where T : IRepository
        {
            var interfaceType = typeof(T);
            if (_repositories.ContainsKey(interfaceType))
                return (T)_repositories[interfaceType];

            var repositoryType = this.GetType().Assembly.GetTypes().SingleOrDefault(t => interfaceType.IsAssignableFrom(t));
            if (repositoryType == null)
                repositoryType = typeof(DomainUnitOfWork<>).Assembly.GetTypes().SingleOrDefault(t => interfaceType.IsAssignableFrom(t));

            if (repositoryType == null)
                return default(T);

            T repository = default(T);

            var contextConstructor = repositoryType.GetConstructor(new Type[] { _dbContext.GetType() });
            if (contextConstructor != null)
                repository = (T)contextConstructor.Invoke(new object[] { _dbContext });

            // попытка создать репозиторий из пустого конструктора
            if (repository == null)
            {
                var defaultConstructor = repositoryType.GetConstructor(Type.EmptyTypes);
                if (defaultConstructor != null)
                    repository = (T)defaultConstructor.Invoke(null);
            }

            // если удалось создать репозиторий -> записываем его в коллекцию
            if (repository != null)
                _repositories.Add(interfaceType, repository);

            return repository;
        }

    
        public ICollection<T> GetEntities<T>(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
            where T : class, IEntity   //where T : IdentifiableEntity<int>
        {
            var query = this.DbContext.Set<T>().AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        public TEntity GetEntity<TEntity>(object key)
            where TEntity : class, IEntity
        {
            return this.DbContext.Set<TEntity>().Find(key);
        }

        public void AddEntity<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            this.DbContext.Set<TEntity>().Add(entity);
        }

        public void UpdateEntity<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            var entry = this.DbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Modified;
        }

        public void DeleteEntity<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            var entry = this.DbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Deleted;
        }


        //public void TransactionBegin(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        //{
        //    if (_transactions != null && _transactions.Count > 0)
        //        throw new CustomExecutionException("Внимание, уже существуют созданые транзакции на данном контексте! Нельзя задублировать транзакции!");

        //    _transactions = new List<DbContextTransaction>();
        //    var transaction = _dbContext.Database.BeginTransaction(isolationLevel);
        //    _transactions.Add(transaction);
        //}

        //public void TransactionCommit()
        //{
        //    foreach (var transaction in _transactions)
        //    {
        //        transaction.Commit();
        //        transaction.Dispose();
        //    }
        //    _transactions = null;
        //}

        //public void TransactionRollback()
        //{
        //    if (_transactions == null)
        //        return;

        //    foreach (var transaction in _transactions)
        //    {
        //        transaction.Rollback();
        //        transaction.Dispose();
        //    }
        //    _transactions = null;
        //}


        //public IEnumerable<T> SqlQuery<T>(string sql)
        //{
        //    return _dbContext.Database.SqlQuery<T>(sql).ToList();
        //}

        //public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        //{
        //    return _dbContext.Database.SqlQuery<T>(sql, parameters).ToList();
        //}
    }
}
