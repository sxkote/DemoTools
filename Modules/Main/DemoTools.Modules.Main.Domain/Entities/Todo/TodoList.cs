using SX.Common.Domain.Entities;
using SX.Common.Domain.Interfaces;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Domain.Entities.Todo
{
    public class TodoList : EntityGuid, ISubscription, IAccessibleEntity
    {
        public Guid SubscriptionID { get; protected set; }
        public string Title { get; protected set; }

        public List<TodoItem> Items { get; protected set; }

        protected TodoList()
        {
            this.Items = new List<TodoItem>();
        }

        public bool HasAccess(IToken token)
        {
            return token != null && token.SubscriptionID == this.SubscriptionID;
        }

        public void Change(string title)
        {
            this.Title = title;
        }

        static public TodoList Create(Guid subscriptionID, string title)
        {
            return new TodoList()
            {
                ID = CommonService.NewGuid,
                SubscriptionID = subscriptionID,
                Title = title
            };
        }

    
    }
}
