using SX.Common.Domain.Entities;
using SX.Common.Shared.Enums;
using SX.Common.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTools.Modules.Main.Domain.Entities.Persons
{
    public class Person :EntityGuid
    {
        public PersonFullName Name { get; protected set; }
        public Gender Gender { get; protected set; }
        public DateTime? BirthDate { get; protected set; }
        public string Cellular { get; protected set; }
        public string Email { get; protected set; }


        public virtual User User { get; protected set; }
    }
}
