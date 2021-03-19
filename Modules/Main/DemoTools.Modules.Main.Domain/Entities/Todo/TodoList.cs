using SX.Common.Domain.Entities;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Domain.Entities.Todo
{
    public class TodoList : EntityGuid
    {
        public string Title { get; protected set; }

        public List<TodoItem> Items { get; protected set; }

        protected TodoList()
        {
            this.Items = new List<TodoItem>();
        }

        public void Change(string title)
        {
            this.Title = title;
        }

        static public TodoList Create(string title)
        {
            return new TodoList()
            {
                ID = CommonService.NewGuid,
                Title = title
            };
        }
    }
}
