using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities.Todo;
using SX.Common.Domain.Services;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void CreateListItem(Guid listID, string title, DateTime dueDate)
        {
            var list = this.TodoListRepo.GetTracking(listID);

            var item = TodoItem.Create(title);
            list.Items.Add(item);

            //this.TodoListRepo.Update(list);
            this.SaveChanges();
        }

        public void ChangeListItem(Guid listID, Guid itemID, string title, DateTime dueDate)
        {
            var list = this.TodoListRepo.GetTracking(listID);

            var item = list.Items.FirstOrDefault(i => i.ID == itemID);
            item.Change(title);

            this.TodoListRepo.Update(list);
            this.SaveChanges();
        }

        public void ToggleListItem(Guid listID, Guid itemID)
        {
            var list = this.TodoListRepo.GetTracking(listID);

            var item = list.Items.FirstOrDefault(i => i.ID == itemID);
            item.ToggleMark();

            this.TodoListRepo.Update(list);
            this.SaveChanges();
        }

        public void DeleteListItem(Guid listID, Guid itemID)
        {
            var list = this.TodoListRepo.GetTracking(listID);

            var item = list.Items.FirstOrDefault(i => i.ID == itemID);
            list.Items.Remove(item);

            this.TodoListRepo.Update(list);
            this.SaveChanges();
        }
    }
}
