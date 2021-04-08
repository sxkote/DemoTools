using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Shared.DomainEvents;
using Microsoft.Extensions.Configuration;
using SX.Common.Infrastructure.Services;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;

namespace DemoTools.Modules.Main.Infrastructure.Services
{
    public class MainNotificationService : IMainNotificationService,
        IDomainEventHandler<PersonRegistrationInited>
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

        public MainNotificationService(IConfiguration configuration)
        {
            _emailService = new EmailNotificationService(configuration[CONFIG_NAME_SMTP_SERVER]);
        }

        public void Dispose()
        {
            _emailService = null;
        }

        //private IEmailNotificationService GetEmailService()
        //{
        //    if (_emailService != null)
        //        return _emailService;

        //    var config = _settingsProvider.GetSettings(CONFIG_NAME_SMTP_SERVER);
        //    _emailService = new EmailNotificationService(config);

        //    return _emailService;
        //}

        public void Handle(PersonRegistrationInited args)
        {
            if (args == null)
                return;

            this.EmailService.SendEmail("Demo-Tools Registration", $"Dear {args.NameFirst} {args.NameLast}, To continue registration process in Demo-Tools for login '{args.Login}', please use PIN = {args.PIN}.", args.Email);
        }
    }
}
