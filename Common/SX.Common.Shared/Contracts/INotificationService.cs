using SX.Common.Shared.Models;
using System.Collections.Generic;

namespace SX.Common.Shared.Contracts
{
    public interface INotificationService
    {
    }

    public interface IEmailNotificationService : INotificationService
    {
        void SendEmail(Message message);
        void SendEmail(string subject, string text, Recipients recipients, string sender = "", IEnumerable<FileData> attachments = null, IEnumerable<FileData> resources = null);
    }

    public interface ISmsNotificationService : INotificationService
    {
        void SendSms(string phone, string text);
    }
}
