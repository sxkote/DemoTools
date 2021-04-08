using SX.Common.Domain.Entities;
using SX.Common.Shared.Enums;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Models;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTools.Modules.Main.Domain.Entities.Persons
{
    public class Person : EntityGuid, ISubscription
    {
        public Guid SubscriptionID { get; protected set; }
        public PersonFullName Name { get; protected set; }
        public Gender Gender { get; protected set; }
        public DateTime? BirthDate { get; protected set; }
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

        static public Person Create(Guid subscriptionID, PersonFullName name, string cellular, string email, Gender gender = Gender.Unknown, DateTime? birthdate = null)
        {
            return new Person()
            {
                ID = CommonService.NewGuid,
                SubscriptionID = subscriptionID,
                Name = name,
                Cellular = cellular ?? "",
                Email = email ?? "",
                Gender = gender,
                BirthDate = birthdate
            };
        }

    }
}
