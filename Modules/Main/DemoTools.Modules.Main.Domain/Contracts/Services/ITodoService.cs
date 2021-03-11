using DemoTools.Modules.Main.Domain.Entities.Todos;
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
    }
}
