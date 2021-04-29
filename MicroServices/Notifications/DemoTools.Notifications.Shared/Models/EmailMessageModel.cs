namespace DemoTools.Notifications.Shared.Models
{
    public class EmailMessageModel
    {
        public const string EMAIL_QUEUE_NAME = "demo-tools-notifications-queue";

        public string Recipients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
