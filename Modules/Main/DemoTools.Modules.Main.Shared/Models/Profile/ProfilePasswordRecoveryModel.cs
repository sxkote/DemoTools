using System;

namespace DemoTools.Modules.Main.Shared.Models.Profile
{
    public class ProfilePasswordRecoveryModel
    {
        public Guid? PersonID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public bool IsPasswordMatch() => this.Password == this.PasswordConfirm;
    }
}
