using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SX.Common.Domain.Contracts;
using SX.Common.Domain.Entities;
using SX.Common.Domain.Interfaces;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SX.Common.Infrastructure.Services
{
    public class DomainRepository<TDbContext, TEntity, TKey> : Repository<TDbContext, TEntity>, IDomainRepository<TEntity, TKey>
      where TDbContext : DbContext, new()
       where TEntity : Entity<TKey>
    {
        protected virtual IQueryable<TEntity> GetQuery(bool multiple, bool tracking = false)
        {
            IQueryable<TEntity> query = this.DbSet;

            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual IQueryable<TEntity> QuerySingle => this.GetQuery(false, false);
        public virtual IQueryable<TEntity> QueryAll => this.GetQuery(true, false);

        public DomainRepository(TDbContext context)
            : base(context) { }

        public override IEnumerable<TEntity> GetAll()
        {
            return this.QueryAll.ToList();
        }

        public virtual TEntity Get(TKey id)
        {
            var result = this.GetEntityByKey(this.QuerySingle, id);

            if (result != null && result is IDeletableEntity)
            {
                if (((IDeletableEntity)result).IsDeleted)
                    return null;
            }

            return result;
        }

        public virtual TEntity Modify(TEntity entity)
        {
            if (!entity.ID.Equals(default(TKey)))
                return this.Update(entity);

            return this.Add(entity);
        }

        public virtual void Delete(TKey obj)
        {
            var entity = this.Get(obj);

            // if not found - assume already deleted...
            if (entity == null) return;

            base.Delete(entity);
        }

        protected TEntity GetEntityByKey(IQueryable<TEntity> query, object key)
        {
            if (key == null)
                return null;

            if (key is TKey)
                return query.SingleOrDefault(this.BuildFindExpression("ID", key));
            else
            {
                var code = key.ToString();
                if (!String.IsNullOrEmpty(code))
                {
                    if (typeof(ICoded).IsAssignableFrom(typeof(TEntity)))
                        return query.SingleOrDefault(this.BuildFindExpression("Code", code));
                    if (typeof(INamed).IsAssignableFrom(typeof(TEntity)))
                        return query.SingleOrDefault(this.BuildFindExpression("Name", code));
                }
            }

            return null;
        }

        public virtual TEntity GetTracking(TKey id, bool multiple = false)
        {
            var entity = this.GetEntityByKey(this.GetQuery(multiple, true), id);

            this.Refresh(entity);

            return entity;
        }

        public virtual IEnumerable<TEntity> GetTracking(IEnumerable<TKey> ids, bool multiple = false)
        {
            var result = new List<TEntity>();

            CommonService.RangeForEach(ids.ToList(), MAX_ENTITIES_TO_QUERY, (range) =>
            {
                var items = this.GetQuery(multiple, true)
                    .Where(r => range.Contains(r.ID))
                    .ToList();

                result.AddRange(items);
            });

            return result;
        }


        protected static string ToString(Guid? guid)
        {
            if (guid.HasValue)
                return $"'{guid.ToString()}'";

            return "NULL";
        }

        protected static string ToString(bool? flag)
        {
            if (flag.HasValue)
                if (flag.Value == true)
                    return "1";
                else return "0";

            return "NULL";

        }

        protected static string ToString(string value)
        {
            if (value == null)
                return "NULL";

            return $"'{value.Replace("'", "''")}'";
        }

        protected static string ToString(DateTime date) => $"'{date.ToString("yyyy-MM-dd")}'";

        protected static string ToString(DateTime? date)
        {
            if (date.HasValue)
                return $"'{date.Value.ToString("yyyy-MM-dd")}'";

            return "NULL";
        }

        protected static string ToString(DateTimeOffset? date)
        {
            if (date.HasValue)
                return $"'{date.Value.ToString("yyyy-MM-dd HH:mm:ss zzz")}'";

            return "NULL";
        }
    }

    public class DomainRepository32<TDbContext, TEntity> : DomainRepository<TDbContext, TEntity, int>
       where TDbContext : DbContext, new()
       where TEntity : Entity32
    {
        public DomainRepository32(TDbContext context)
            : base(context) { }

    }

    public class DomainRepository64<TDbContext, TEntity> : DomainRepository<TDbContext, TEntity, long>
         where TDbContext : DbContext, new()
         where TEntity : Entity64
    {
        public DomainRepository64(TDbContext context)
            : base(context) { }

    }

    public class DomainRepositoryGuid<TDbContext, TEntity> : DomainRepository<TDbContext, TEntity, Guid>
         where TDbContext : DbContext, new()
         where TEntity : EntityGuid
    {
        public DomainRepositoryGuid(TDbContext context)
            : base(context) { }

    }
}
