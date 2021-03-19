using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SX.Common.Domain.Enums;
using SX.Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SX.Common.Infrastructure.Data
{
    public class DomainDbContext : DbContext
    {
        public const int TIMEOUT = 180;

        public static Action<string, string> Log = null;

        //protected const bool _proxyCreationEnabled = false;
        protected const bool _lazyLoadingEnabled = false;

        protected List<object> _objects = new List<object>();

        public DomainDbContext()
            : base()
        {
            this.ChangeConfiguration();
        }

        public DomainDbContext(DbContextOptions options)
            : base(options)
        {
            this.ChangeConfiguration();
        }

        protected void ChangeConfiguration()
        {
            this.ChangeTracker.LazyLoadingEnabled = _lazyLoadingEnabled;
            //this.Configuration.ProxyCreationEnabled = _proxyCreationEnabled;

            //this.Database.CommandTimeout = TIMEOUT;

            //#if DEBUG
            //            this.Database.Log = (s) =>
            //            {
            //                System.Diagnostics.Debug.WriteLine($"{Environment.NewLine}{s}");

            //                //if (CommonDbContext.Log != null)
            //                //    CommonDbContext.Log(_id, s);
            //            };
            //#endif
        }

        protected List<EntityEntry<T>> GetEntries<T>()
            where T : class
        {
            var result = this.ChangeTracker
                       .Entries<T>()
                       .ToList();

            // добавляем кастомные объекты этого типа
            result.AddRange(_objects.OfType<T>().Select(o => this.Entry(o)).ToList());

            return result;
        }

        //protected void MatchDbState(IDbState obj)
        //{
        //    if (obj == null || obj.DbState == DbState.None)
        //        return;

        //    this.Entry(obj).State = ConvertEntityState(obj.DbState);
        //}

        protected void MatchDbStateForAllEnties()
        {
            // get DbStated Enties
            this.GetEntries<IDbState>()
                .Where(e => e.Entity.DbState != DbState.None)
                .ToList()
                .ForEach(e => e.State = ConvertEntityState(e.Entity.DbState));
        }

        protected void ClearDbStateForAllEntries()
        {
            this.GetEntries<IDbState>()
                .ForEach(e => e.Entity.ChangeDbState(DbState.None));
        }

        public override int SaveChanges()
        {
            //this.MatchDbStateForAllEnties();

            //// MAIN save changes
            //if (AppSettings.Global.UseZZZ)
            //    this.BatchSaveChanges();
            //else
                base.SaveChanges();
            //this.BulkSaveChanges();

            // set back DbStates to None (after saving)
            //this.ClearDbStateForAllEntries();

            return 0;
        }

        static protected EntityState ConvertEntityState(DbState state)
        {
            switch (state)
            {
                case DbState.Added:
                    return EntityState.Added;
                case DbState.Modified:
                    return EntityState.Modified;
                case DbState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
    }
}
