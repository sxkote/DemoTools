using SX.Common.Shared.Interfaces;

namespace DemoTools.Core.Shared.DomainEvents
{
    public class PasswordRecoveryInitDomainEvent : IDomainEvent
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string PIN { get; set; }
    }
}
