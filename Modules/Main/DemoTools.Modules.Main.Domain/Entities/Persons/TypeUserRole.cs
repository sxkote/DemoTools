using SX.Common.Domain.Entities;
using SX.Common.Shared;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Modules.Main.Domain.Entities.Persons
{
    public class TypeUserRole : EntityGuid, INamed
    {
        public string Name { get; protected set; }
        public string Title { get; protected set; }
        public List<TypeUserRolePermission> RolePermissions { get; protected set; }


        protected TypeUserRole()
        {
            this.RolePermissions = new List<TypeUserRolePermission>();
        }

        public bool HasPermission(string permission)
        {
            return this.RolePermissions.Any(p => p.Permission.Name.Equals(permission, CommonService.StringComparison));
        }

        public void Change(string name, string title, List<TypeUserPermission> permissions)
        {
            this.Change(name, title);
            this.Change(permissions);
        }

        public void Change(string name, string title)
        {
            this.Name = name ?? "";
            this.Title = title ?? "";
        }

        public void Change(IEnumerable<TypeUserPermission> permissions)
        {
            foreach (var permission in permissions)
                if (!this.HasPermission(permission.Name))
                    this.RolePermissions.Add(TypeUserRolePermission.Create(this, permission));

            this.RolePermissions.Remove(p => !permissions.Any(permission => permission.Name.Equals(p.Permission.Name, CommonService.StringComparison)));
        }


        static public TypeUserRole Create(string name, string title)
        {
            var role = new TypeUserRole()
            {
                ID = CommonService.NewGuid,
                Name = name ?? "",
                Title = title ?? ""
            };

            return role;
        }
    }

    public class TypeUserRolePermission
    {
        public Guid UserRoleID { get; private set; }
        public TypeUserRole Role { get; private set; }

        public Guid UserPermissionID { get; private set; }
        public TypeUserPermission Permission { get; private set; }

        static public TypeUserRolePermission Create(TypeUserRole role, TypeUserPermission permission)
        {
            return new TypeUserRolePermission()
            {
                UserRoleID = role.ID,
                Role = role,
                UserPermissionID = permission.ID,
                Permission = permission
            };
        }
    }
}
