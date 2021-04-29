using SX.Common.Infrastructure.Models;
using SX.Common.Shared;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Enums;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Models;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SX.Common.Infrastructure.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        protected SMTPServerConfig _config;

        public virtual string DefaultFrom => _config.From ?? "";


        public EmailNotificationService(SMTPServerConfig config)
        {
            _config = config;
        }


        public SmtpClient BuildSMTPClient()
        {
            if (_config == null || _config.IsDefault())
                return new SmtpClient();

            var client = new SmtpClient(_config.Server, _config.Port);
            client.Credentials = new NetworkCredential(_config.Login, _config.Password);
            client.EnableSsl = _config.SSL;

            return client;
        }

        protected virtual List<string> GetRecipents(Recipients recipients)
        {
            if (recipients == null)
                return new List<string>();

            return recipients
                .Where(r => r.Type == RecipientType.Email)
                .Select(r => r.Address)
                .Where(a => !String.IsNullOrWhiteSpace(a))
                .ToList();
        }

        protected virtual MailMessage BuildMessage(string from, Recipients recipients, IEnumerable<FileData> attachments = null)
        {
            var message = new MailMessage();

            if (!String.IsNullOrWhiteSpace(from))
            {
                var sender = new MailAddress(from);

                message.From = sender;
                message.Sender = sender;
                message.ReplyToList.Add(sender);
            }
            else if (!string.IsNullOrWhiteSpace(this.DefaultFrom))
            {
                message.From = new MailAddress(this.DefaultFrom);
            }

            // получатели Email сообщения
            var messageRecipents = this.GetRecipents(recipients);
            if (messageRecipents == null || messageRecipents.Count <= 0)
                return null;

            foreach (var r in messageRecipents)
                message.To.Add(r);


            if (attachments != null)
                foreach (var file in attachments)
                    message.Attachments.Add(new Attachment(new MemoryStream(file.Data), file.FileName));

            return message;
        }

        protected List<LinkedResource> BuildLinkedResources(ref string content, IEnumerable<FileData> files = null)
        {
            var result = new List<LinkedResource>();

            if (files == null)
                return result;

            foreach (var file in files)
            {
                // create new Linked Resource
                var linkedResource = new LinkedResource(new MemoryStream(file.Data), CommonService.GetMimeMapping(file.FileName));

                // generate new GUID to Resource
                linkedResource.ContentId = CommonService.NewGuid.ToString();

                // add Resource to the result collection
                result.Add(linkedResource);

                // replace File link (href) to Resource GUID
                content = Regex.Replace(content, $"=\"{file.FileName}\"", $"=\"cid:{linkedResource.ContentId}\"");
            }

            return result;
        }

        protected AlternateView BuildHtmlView(string htmlText, IEnumerable<FileData> htmlResources = null)
        {
            // get content to modify by internal resources links
            var viewContent = htmlText;

            // get Linked Resources from files
            var viewResources = this.BuildLinkedResources(ref viewContent, htmlResources);

            // create new HTML View 
            var view = AlternateView.CreateAlternateViewFromString(viewContent, null, "text/html");

            // add all Linked Resources to View
            foreach (var res in viewResources)
                view.LinkedResources.Add(res);

            return view;
        }

        public void SendEmail(Message message)
        {
            if (message == null)
                throw new CustomArgumentException("Не задано сообщение для отправки e-mail!");

            if (message.Recipients == null || message.Recipients.Count <= 0)
                return;

            var email = this.BuildMessage(message.Sender, message.Recipients, message.Attachments);
            if (email == null)
                return;

            email.Subject = message.Subject.Trim();
            email.IsBodyHtml = message.Text.IsHtml();

            if (email.IsBodyHtml)
            {
                // add HTML View to the Message
                email.AlternateViews.Add(this.BuildHtmlView(message.Text, message.Resources));

                //email.Body = alternativeText ?? "";
            }
            else
            {
                email.Body = message.Text;
            }

            using (var smtp = this.BuildSMTPClient())
            {
                smtp.Send(email);
            }
        }

        public void SendEmail(string subject, string text, Recipients recipients, string sender = "", IEnumerable<FileData> attachments = null, IEnumerable<FileData> resources = null)
        {
            var message = new Message(subject, text, recipients, sender);
            message.AddAttachments(attachments);
            message.AddResources(resources);
            this.SendEmail(message);
        }
    }
}
