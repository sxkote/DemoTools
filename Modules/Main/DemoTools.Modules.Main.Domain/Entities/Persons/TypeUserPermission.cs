using SX.Common.Domain.Entities;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;

namespace DemoTools.Modules.Main.Domain.Entities.Persons
{
    public class TypeUserPermission : EntityGuid, INamed
    {
        public string Name { get; protected set; }
        public string Title { get; protected set; }
        public string Category { get; protected set; }

        protected TypeUserPermission() { }

        static public TypeUserPermission Create(string name, string title, string category)
        {
            return new TypeUserPermission()
            {
                ID = CommonService.NewGuid,
                Name = name,
                Title = title ?? "",
                Category = category ?? ""
            };
        }
    }
}
