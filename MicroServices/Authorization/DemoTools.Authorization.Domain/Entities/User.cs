using SX.Common.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Authorization.Domain.Entities
{
    public class User : EntityGuid
    {
        public string Login { get; protected set; }
        public string Password { get; protected set; }
        public List<UserRole> UserRoles { get; protected set; }

        protected User()
        {
            this.UserRoles = new List<UserRole>();
        }

        public void ChangePassword(string passwordHash)
        {
            this.Password = passwordHash;
        }

        static public User Create(Guid id, string login, string password, IEnumerable<TypeUserRole> roles = null)
        {
            var user = new User()
            {
                ID = id,
                Login = login,
                Password = password
            };

            if (roles != null && roles.Count() > 0)
                foreach (var role in roles)
                {
                    var userRole = UserRole.Create(user, role);
                    user.UserRoles.Add(userRole);
                }

            return user;
        }

    }

    public class UserRole
    {
        public Guid UserID { get; protected set; }
        public User User { get; private set; }

        public Guid UserRoleID { get; private set; }
        public TypeUserRole Role { get; private set; }

        static public UserRole Create(User user, TypeUserRole role)
        {
            return new UserRole()
            {
                UserID = user.ID,
                User = user,
                UserRoleID = role.ID,
                Role = role
            };
        }
    }
}
