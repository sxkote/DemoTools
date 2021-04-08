using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Entities;
using DemoTools.Modules.Main.Infrastructure.Data;
using DemoTools.Modules.Main.Infrastructure.Repositories;
using SX.Common.Infrastructure.Services;
using System;
using System.Linq;

namespace DemoTools.Modules.Main.Infrastructure.Services
{
    public class MainUnitOfWork : DomainUnitOfWork<MainDbContext>, IMainUnitOfWork
    {
        private ITodoListRepository _todoListRepository;
        private ITypeUserRoleRepository _typeUserRoleRepository;
        private IPersonRepository _personRepository;

        public ITodoListRepository TodoListRepository
        {
            get
            {
                if (_todoListRepository == null)
                    _todoListRepository = new TodoListRepository(this.DbContext);
                return _todoListRepository;
            }
        }

        public ITypeUserRoleRepository TypeUserRoleRepository
        {
            get
            {
                if (_typeUserRoleRepository == null)
                    _typeUserRoleRepository = new TypeUserRoleRepository(this.DbContext);
                return _typeUserRoleRepository;
            }
        }

        public IPersonRepository PersonRepository
        {
            get
            {
                if (_personRepository == null)
                    _personRepository = new PersonRepository(this.DbContext);
                return _personRepository;
            }
        }


        public MainUnitOfWork(MainDbContext dbContext)
            : base(dbContext) { }

        //public Activity FindActivity(Guid activityID)
        //{
        //    return this.DbContext.Set<Activity>()
        //        .FirstOrDefault(t => t.ID == activityID);
        //}

        //public void CreateActivity(Activity activity)
        //{
        //    this.DbContext.Set<Activity>().Add(activity);
        //}

    }
}
