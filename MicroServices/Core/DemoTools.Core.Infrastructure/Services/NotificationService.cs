using DemoTools.Core.Shared.DomainEvents;
using DemoTools.Notifications.Shared.Models;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;
using System;

namespace DemoTools.Core.Infrastructure.Services
{
    public class NotificationService :
       IDomainEventHandler<RegistrationInitDomainEvent>,
       IDomainEventHandler<PasswordRecoveryInitDomainEvent>,
       IDomainEventHandler<PasswordChangedDomainEvent>
    {
        private readonly IEventBusPublisher _eventBusPublisher;

        public NotificationService(IEventBusPublisher eventBusPublisher)
        {
            _eventBusPublisher = eventBusPublisher;
        }

        public void Dispose()
        {
            _eventBusPublisher.Dispose();
        }

        private void SendEmail(EmailMessageModel model)
        {
            if (model != null && !String.IsNullOrWhiteSpace(model.Recipients))
                _eventBusPublisher.Publish(model, EmailMessageModel.EMAIL_QUEUE_NAME);
        }

        private void SendEmail(string subject, string body, string recipients)
        {
            this.SendEmail(new EmailMessageModel()
            {
                Subject = subject ?? "",
                Body = body ?? "",
                Recipients = recipients ?? ""
            });
        }

        public void Handle(RegistrationInitDomainEvent args)
        {
            if (args == null)
                return;

            this.SendEmail("Registration", $"Dear {args.NameFirst} {args.NameLast}, \nTo continue registration process in Demo-Tools for login '{args.Login}', \nplease use PIN = {args.PIN}.", args.Email);
        }

        public void Handle(PasswordRecoveryInitDomainEvent args)
        {
            if (args == null)
                return;

            this.SendEmail("Password Recovery", $"Dear {args.NameFirst} {args.NameLast}, \nTo continue password recovery process in Demo-Tools for login '{args.Login}', \nplease use PIN = {args.PIN}.", args.Email);
        }

        public void Handle(PasswordChangedDomainEvent args)
        {
            if (args == null)
                return;

            this.SendEmail("Password Changed", $"Dear {args.NameFirst} {args.NameLast},\nYour password in Demo-Tools for login '{args.Login}', \nhas been changed to '{args.Password}'.", args.Email);
        }
    }
}
