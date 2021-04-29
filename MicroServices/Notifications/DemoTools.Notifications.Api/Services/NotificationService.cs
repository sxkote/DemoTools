using DemoTools.Notifications.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SX.Common.Infrastructure.Models;
using SX.Common.Infrastructure.Services;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Events;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;

namespace DemoTools.Notifications.Api.Services
{
    public class NotificationService : IDomainEventHandler<EventBusMessageReceivedEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotificationService> _logger;

        private IEmailNotificationService _emailService;

        private IEmailNotificationService EmailService
        {
            get
            {
                return _emailService;
            }
        }

        public NotificationService(IConfiguration configuration, ILogger<NotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // connection string from configuration
            var smtpConnectionString = _configuration[SMTPServerConfig.CONFIG_NAME];

            // email service
            _emailService = new EmailNotificationService(new SMTPServerConfig(smtpConnectionString));
        }

        public void Dispose() { }

        private void SendEmail(EmailMessageModel model)
        {
            if (model == null)
                return;

            this.EmailService.SendEmail($"Demo-Tools: {model.Subject}", model.Body, model.Recipients);

            _logger.LogInformation($"Email sent to {model.Recipients}: {model.Subject}");
        }


        public void Handle(EventBusMessageReceivedEvent args)
        {
            if (args == null)
                return;

            var emailModel = CommonService.TryDeserialize<EmailMessageModel>(args.Message);
            if (emailModel != null)
            {
                this.SendEmail(emailModel);
                return;
            }

            var domainEventModel = CommonService.TryDeserialize<IDomainEvent>(args.Message);
            if (domainEventModel != null)
            {
                DomainDispatcher.RaiseEvent(domainEventModel);
                return;
            }
        }
    }
}
