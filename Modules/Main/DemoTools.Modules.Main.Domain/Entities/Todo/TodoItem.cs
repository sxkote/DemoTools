using SX.Common.Domain.Entities;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTools.Modules.Main.Domain.Entities.Todo
{
    public class TodoItem : EntityGuid
    {
        public Guid TodoListID { get; protected set; }
        public string Title { get; protected set; }
        public bool IsDone { get; protected set; }

        protected TodoItem() { }

        public void Change(string title)
        {
            this.Title = title;
        }

        public void ToggleMark()
        {
            this.IsDone = !this.IsDone;
        }

        static public TodoItem Create(string title)
        {
            return new TodoItem()
            {
                ID = CommonService.NewGuid,
                Title = title,
                IsDone = false
            };
        }
    }
}
