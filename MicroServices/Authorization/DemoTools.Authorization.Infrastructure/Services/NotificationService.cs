using DemoTools.Authorization.Shared.DomainEvents;
using Microsoft.Extensions.Configuration;
using SX.Common.Infrastructure.Services;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;

namespace DemoTools.Authorization.Infrastructure.Services
{
    public class NotificationService : 
       IDomainEventHandler<RegistrationInitDomainEvent>,
       IDomainEventHandler<PasswordRecoveryInitDomainEvent>,
       IDomainEventHandler<PasswordChangedDomainEvent>
    {
        public const string CONFIG_NAME_SMTP_SERVER = "SMTPServerConfig";

        //private readonly ISettingsProvider _settingsProvider;
        private IEmailNotificationService _emailService;

        private IEmailNotificationService EmailService
        {
            get
            {
                return _emailService;
            }
        }

        public NotificationService(IConfiguration configuration)
        {
            _emailService = new EmailNotificationService(configuration[CONFIG_NAME_SMTP_SERVER]);
        }

        public void Dispose()
        {
            //_emailService = null;
        }

        public void Handle(RegistrationInitDomainEvent args)
        {
            if (args == null)
                return;

            this.EmailService.SendEmail("Demo-Tools Registration", $"Dear {args.NameFirst} {args.NameLast}, \nTo continue registration process in Demo-Tools for login '{args.Login}', \nplease use PIN = {args.PIN}.", args.Email);
        }

        public void Handle(PasswordRecoveryInitDomainEvent args)
        {
            if (args == null)
                return;

            this.EmailService.SendEmail("Demo-Tools Password Recovery", $"Dear {args.NameFirst} {args.NameLast}, \nTo continue password recovery process in Demo-Tools for login '{args.Login}', \nplease use PIN = {args.PIN}.", args.Email);
        }

        public void Handle(PasswordChangedDomainEvent args)
        {
            if (args == null)
                return;

            this.EmailService.SendEmail("Demo-Tools Password Changed", $"Dear {args.NameFirst} {args.NameLast},\nYour password in Demo-Tools for login '{args.Login}', \nhas been changed to '{args.Password}'.", args.Email);
        }
    }
}
