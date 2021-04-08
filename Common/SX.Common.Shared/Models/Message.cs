using System.Collections.Generic;
using System.Linq;

namespace SX.Common.Shared.Models
{
    public class Message
    {
        public string Sender { get; protected set; } = "";

        public Recipients Recipients { get; protected set; } = new Recipients();

        public string Subject { get; protected set; } = "";
        public string Text { get; protected set; } = "";

        public List<FileData> Attachments { get; protected set; } = new List<FileData>();
        public List<FileData> Resources { get; set; } = new List<FileData>();

        protected Message() { }

        public Message(string subject, string text, Recipients recipients = null, string sender = "")
        {
            this.Subject = subject ?? "";
            this.Text = text ?? "";
            this.Recipients = recipients ?? new Recipients();
            this.Sender = sender ?? "";
        }

        public override string ToString()
        {
            return $"Message: '{this.Subject}'";
        }

        public Message Copy(Recipients recipients)
        {
            return new Message()
            {
                Subject = this.Subject,
                Text = this.Text,

                Sender = this.Sender,
                Recipients = recipients ?? new Recipients(),

                Attachments = new List<FileData>(this.Attachments),
                Resources = new List<FileData>(this.Resources)
            };
        }

        public void AddRecipients(Recipients recipients)
        {
            if (recipients == null)
                return;

            this.Recipients.Add(recipients);
        }
        public void AddRecipients(IEnumerable<Recipient> recipients) => this.AddRecipients(new Recipients(recipients));

        public void AddAttachments(IEnumerable<FileData> attachments)
        {
            if (attachments != null)
                this.Attachments = this.Attachments.Union(attachments).ToList();
        }

        public void AddResources(IEnumerable<FileData> resources)
        {
            if (resources != null)
                this.Resources = this.Resources.Union(resources).ToList();
        }

        public Message FillTemplate(ParamValueCollection collection)
        {
            return new Message()
            {
                Subject = collection?.Replace(this.Subject) ?? this.Subject,
                Text = collection?.Replace(this.Text) ?? this.Text,

                Sender = this.Sender,
                Recipients = new Recipients(this.Recipients),

                Attachments = new List<FileData>(this.Attachments),
                Resources = new List<FileData>(this.Resources)
            };
        }

        public List<FileData> GetAllFiles()
        {
            var result = new List<FileData>();
            if (this.Attachments != null && this.Attachments.Count > 0)
                result.AddRange(this.Attachments);
            if (this.Resources != null && this.Resources.Count > 0)
                result.AddRange(this.Resources);
            return result;
        }
    }
}
