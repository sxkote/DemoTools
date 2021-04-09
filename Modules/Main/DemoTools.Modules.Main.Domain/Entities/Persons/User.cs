using DemoTools.Modules.Main.Shared.Classes;
using SX.Common.Domain.Entities;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Modules.Main.Domain.Entities.Persons
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

        public void ChangePassword(SecurityPolicy policy, string password)
        {
            if (policy == null)
                throw new CustomArgumentException("Security Policy is not provided!");

            if (String.IsNullOrWhiteSpace(password))
                throw new CustomInputException("Password is empty!");

            if (!policy.CheckStrength(password))
                throw new CustomInputException("Password does not match securuty policy!");

            var hash = policy.HashPassword(password);

            if (!String.IsNullOrWhiteSpace(this.Password) && !policy.CanReuse && hash.Equals(this.Password, CommonService.StringComparison))
                throw new CustomInputException("Password was used already!");

            this.Password = hash;
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
