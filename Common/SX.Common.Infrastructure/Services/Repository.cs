using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SX.Common.Domain.Interfaces;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SX.Common.Infrastructure.Services
{
    public class Repository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext, new()
        where TEntity : class
    {
        protected const int MAX_ENTITIES_TO_QUERY = 1000;


        public bool IsAutoSave { get; protected set; }
        public TDbContext DbContext { get; protected set; }
        public DbSet<TEntity> DbSet { get; protected set; }

        public Repository(TDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            this.IsAutoSave = false;

            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<TEntity>();
        }


        protected virtual int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this.DbSet.ToList();
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            this.DbSet.AddRange(entities);
        }

        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity", "Can't add null Entity to DbContext");

            EntityEntry dbEntityEntry = this.DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }

            if (this.IsAutoSave)
                this.SaveChanges();

            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity", "Can't update null Entity to DbContext");

            EntityEntry dbEntityEntry = this.DbContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;

            if (this.IsAutoSave)
                this.SaveChanges();

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity", "Can't delete null Entity to DbContext");

            if (entity is IDeletableEntity)
            {
                ((IDeletableEntity)entity).MarkDeleted();
                this.Update(entity);
                return;
            }

            this.DeleteReferences(entity);

            EntityEntry dbEntityEntry = this.DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }

            if (this.IsAutoSave)
                this.SaveChanges();
        }

        protected virtual void DeleteReferences(TEntity entity)
        {
        }

        public virtual void Refresh(TEntity entity)
        {
            if (entity != null)
            {
                var entry = this.DbContext.Entry(entity);
                if (entry.State != EntityState.Detached && entry.State != EntityState.Added)
                    entry.Reload();
            }
        }

        public virtual void Detach(TEntity entity)
        {
            var entry = this.DbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Detached;
        }



        protected void LoadNavigation(TEntity entity, Expression<Func<TEntity, object>> navigation)
        {
            var entry = this.DbContext.Entry(entity);
            if (entity != null && navigation != null)
                entry.Reference(navigation).Load();
        }

        protected void LoadCollection(TEntity entity, Expression<Func<TEntity, IEnumerable<object>>> collection)
        {
            var entry = this.DbContext.Entry(entity);
            if (entity != null && collection != null)
                entry.Collection(collection).Load();
        }

        /// <summary>
        /// Builds Lambda Expression for IQuerable&lt;T&gt; (DbSet) to Find the entity by Field and Value
        /// </summary>
        /// <param name="field">Name of the field to search</param>
        /// <param name="value">Value of the field to search</param>
        /// <returns>Lambda Expression to make a query</returns>
        protected Expression<Func<TEntity, bool>> BuildFindExpression(string field, object value)
        {
            var entityExpression = Expression.Parameter(typeof(TEntity));

            var fieldExpression = Expression.Equal(Expression.Property(entityExpression, field), Expression.Constant(value));

            return Expression.Lambda<Func<TEntity, bool>>(fieldExpression, entityExpression);
        }
    }
}
