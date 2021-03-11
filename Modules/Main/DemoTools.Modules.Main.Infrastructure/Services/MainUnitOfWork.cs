using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Infrastructure.Data;
using DemoTools.Modules.Main.Infrastructure.Repositories;
using SX.Common.Infrastructure.Services;

namespace DemoTools.Modules.Main.Infrastructure.Services
{
    public class MainUnitOfWork : DomainUnitOfWork<MainDbContext>, IMainUnitOfWork
    {
        private ITodoListRepository _todoListRepository;

        public ITodoListRepository TodoListRepository
        {
            get
            {
                if (_todoListRepository == null)
                    _todoListRepository = new TodoListRepository(this.DbContext);
                return _todoListRepository;
            }
        }

        public MainUnitOfWork(MainDbContext dbContext)
            : base(dbContext) { }
    }
}
