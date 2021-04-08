using SX.Common.Domain.Entities;
using SX.Common.Shared.Services;
using System;

namespace DemoTools.Modules.Main.Domain.Entities
{
    public class Subscription : Entity<Guid>
    {
        public string Title { get; protected set; }

        protected Subscription() { }

        public override string ToString()
        {
            return String.IsNullOrWhiteSpace(this.Title) ? this.ID.ToString() : this.Title;
        }


        static public Subscription Create(string title)
        {
            return new Subscription()
            {
                ID = CommonService.NewGuid,
                Title = title
            };
        }
    }
}
