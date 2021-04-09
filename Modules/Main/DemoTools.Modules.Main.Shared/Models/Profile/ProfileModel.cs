using System;

namespace DemoTools.Modules.Main.Shared.Models.Profile
{
    public class ProfileModel
    {
        public Guid PersonID { get; set; }
        public Guid SubscriptionID { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Telephone { get; set; }
    }
}
