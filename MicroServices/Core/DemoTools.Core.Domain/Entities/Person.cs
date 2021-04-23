using SX.Common.Domain.Entities;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Models;
using System;

namespace DemoTools.Core.Domain.Entities
{
    public class Person : EntityGuid, ISubscription
    {
        public Guid SubscriptionID { get; protected set; }
        public PersonFullName Name { get; protected set; }
        public string Cellular { get; protected set; }
        public string Email { get; protected set; }


        public virtual User User { get; protected set; }

        protected Person() { }

        public void SetUser(User user)
        {
            if (this.User != null)
                throw new CustomOperationException("Пользователь уже задан!");

            this.User = user;
        }

        static public Person Create(Guid id, Guid subscriptionID, PersonFullName name, string cellular, string email)
        {
            return new Person()
            {
                ID = id,
                SubscriptionID = subscriptionID,
                Name = name,
                Cellular = cellular ?? "",
                Email = email ?? "",
            };
        }
    }
}