using SX.Common.Shared.Enums;
using System;

namespace DemoTools.Modules.Main.Shared.Models.Profile
{
    public class ProfileRegistrationModel
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
        public DateTime? BirthDate { get; set; } = null;


        public string GetValidLogin() => String.IsNullOrWhiteSpace(this.Login) ? this.Email : this.Login;

        public bool IsPasswordMatch() => this.Password == this.PasswordConfirm;
    }
}
