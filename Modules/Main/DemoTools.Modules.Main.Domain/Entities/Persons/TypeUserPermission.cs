using SX.Common.Domain.Entities;
using SX.Common.Shared.Interfaces;

namespace DemoTools.Modules.Main.Domain.Entities.Persons
{
    public class TypeUserPermission : EntityGuid, INamed
    {
        public string Name { get; protected set; }
        public string Title { get; protected set; }
        public string Category { get; protected set; }

        protected TypeUserPermission() { }
    }
}
