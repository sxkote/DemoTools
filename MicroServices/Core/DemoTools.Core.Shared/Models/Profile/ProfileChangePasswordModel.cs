namespace DemoTools.Core.Shared.Models.Profile
{
    public class ProfileChangePasswordModel
    {
        public string PasswordCurrent { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public bool IsPasswordMatch() => this.Password == this.PasswordConfirm;
    }
}
