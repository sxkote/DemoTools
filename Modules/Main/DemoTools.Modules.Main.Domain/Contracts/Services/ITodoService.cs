using DemoTools.Modules.Main.Domain.Entities.Todo;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Domain.Contracts.Services
{
    public interface ITodoService
    {
        IEnumerable<TodoList> GetAllLists();
        TodoList GetList(Guid listID);
        TodoList CreateList(string title);
        void ChangeList(Guid listID, string title);
        void DeleteList(Guid listID);


        void CreateListItem(Guid listID, string title, DateTime dueDate);
        void ChangeListItem(Guid listID, Guid itemID, string title, DateTime dueDate);
        void ToggleListItem(Guid listID, Guid itemID);
        void DeleteListItem(Guid listID, Guid itemID);
    }
}
