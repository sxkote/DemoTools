using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities.Todos;
using SX.Common.Domain.Services;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Domain.Services
{
    public class TodoService : DomainService<IMainUnitOfWork>, ITodoService
    {
        protected ITodoListRepository TodoListRepo => this.UnitOfWork.TodoListRepository;

        public TodoService(IMainUnitOfWork unitOfWork, ITokenProvider tokenProvider)
            :base(unitOfWork, tokenProvider)
        {
        }

        public IEnumerable<TodoList> GetAllLists()
        {
            var token = _tokenProvider.GetToken();
            return this.TodoListRepo.GetAll();
        }
        public TodoList GetList(Guid listID)
        {
            return this.TodoListRepo.Get(listID);
        }
        public TodoList CreateList(string title)
        {
            var list = TodoList.Create(title);
            
            this.TodoListRepo.Add(list);
            this.SaveChanges();

            return list;
        }
        public void ChangeList(Guid listID, string title)
        {
            var list = this.TodoListRepo.GetTracking(listID);

            list.Change(title);

            this.TodoListRepo.Update(list);
            this.SaveChanges();
        }
        public void DeleteList(Guid listID)
        {
            this.TodoListRepo.Delete(listID);
            this.SaveChanges();
        }

    }
}
