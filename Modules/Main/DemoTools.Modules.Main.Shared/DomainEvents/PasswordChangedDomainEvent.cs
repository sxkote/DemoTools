using SX.Common.Shared.Interfaces;

namespace DemoTools.Modules.Main.Shared.DomainEvents
{
    public class PasswordChangedDomainEvent : IDomainEvent
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Password { get; set; }
    }
}
