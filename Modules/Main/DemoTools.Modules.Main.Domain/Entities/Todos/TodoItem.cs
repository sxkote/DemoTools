using SX.Common.Domain.Entities;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTools.Modules.Main.Domain.Entities.Todos
{
    public class TodoItem : EntityGuid
    {
        public string Title { get; protected set; }
        public bool IsDone { get; protected set; }

        protected TodoItem() { }

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
