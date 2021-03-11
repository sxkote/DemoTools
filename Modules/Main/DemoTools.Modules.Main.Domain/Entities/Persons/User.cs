using DemoTools.Modules.Main.Shared.Classes;
using SX.Common.Domain.Entities;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;

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

        public void ChangePassword(SecurityPolicy policy, string password, bool resetPasswordDate)
        {
            if (policy == null)
                throw new CustomArgumentException("Не заданы политики авторизации!");

            if (String.IsNullOrWhiteSpace(password))
                throw new CustomInputException("Не задан пароль для изменения!");

            if (!policy.CheckStrength(password))
                throw new CustomInputException("Пароль не отвечает требованиям безопасности!");

            var hash = policy.HashPassword(password);

            if (!String.IsNullOrWhiteSpace(this.Password) && !policy.CanReuse && hash.Equals(this.Password, CommonService.StringComparison))
                throw new CustomInputException("Пароль уже был использован ранее!");

            this.Password = hash;

            //if (resetPasswordDate)
            //    this.ResetPasswordDate();
            //else
            //    this.PasswordDate = policy.NextPasswordDate();
        }

    }

    public class UserRole
    {
        public Guid UserID { get; protected set; }
        public User User { get; private set; }

        public Guid UserRoleID { get; private set; }
        public TypeUserRole Role { get; private set; }
    }
}
