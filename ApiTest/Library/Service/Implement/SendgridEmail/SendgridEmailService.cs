using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Logs;
using Service.Interface.SendEmail;
using Service.Models.SendgridEmail;
using System.Configuration;
using SendGrid;
using System.Net.Mail;
using Common.Helpers;
using System.IO;
using System.Data.Entity;
using System.Linq;

namespace Service.Implement.SendgridEmail
{
    public class SendgridEmailService : BaseServiceWithLogging, IEmailService
    {
        private static readonly string ApiKey = ConfigurationManager.AppSettings["sendgridapikey"];
        private static readonly string Environment = ConfigurationManager.AppSettings["Environment"];
        private static readonly string FromEmail = ConfigurationManager.AppSettings["FromEmail"];
        private static readonly string FromName = ConfigurationManager.AppSettings["FromName"];

        private readonly DbContext _context;

        public SendgridEmailService(DbContext context,INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
        }
        public Task SendEmailAsync(List<string> to, string bcc, string cc, string subject, string body)
        {
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(to);
            if (!string.IsNullOrEmpty(bcc))
                myMessage.AddCc(bcc);
            if (!string.IsNullOrEmpty(cc))
                myMessage.AddCc(cc);

            myMessage.From = new MailAddress(FromEmail, FromName);
            myMessage.Subject = subject;
            myMessage.Html = body;

            // Create a Web transport, using API Key
            var transportWeb = new Web(ApiKey);

            // Send the email.
            return transportWeb.DeliverAsync(myMessage);
        }

        public void SendEmail(List<string> to, string bcc, string cc, string subject, string body)
        {
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(to);
            if (!string.IsNullOrEmpty(bcc))
                myMessage.AddCc(bcc);
            if (!string.IsNullOrEmpty(cc))
                myMessage.AddCc(cc);

            myMessage.From = new MailAddress(FromEmail, FromName);
            myMessage.Subject = subject;
            myMessage.Html = body;

            // Create a Web transport, using API Key
            var transportWeb = new Web(ApiKey);

            // Send the email.
            AsyncHelper.RunSync(() => transportWeb.DeliverAsync(myMessage));
        }

        public Task SendEmailWithAttachmentsAsync(List<string> to, string bcc, string cc, string subject, string body, List<AttachmentModel> attachments)
        {
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(to);
            if (!string.IsNullOrEmpty(bcc))
                myMessage.AddCc(bcc);
            if (!string.IsNullOrEmpty(cc))
                myMessage.AddCc(cc);

            myMessage.From = new MailAddress(FromEmail, FromName);
            myMessage.Subject = (Environment != null? $"{Environment}_{subject}":subject);
            myMessage.Html = body;
            foreach (var attachment in attachments)
            {
                if (string.IsNullOrEmpty(Environment))
                    myMessage.AddAttachment(attachment.Stream, attachment.Name);
                else
                    myMessage.AddAttachment(attachment.Stream, $"{Environment}_{attachment.Name}");
            }
            // Create a Web transport, using API Key
            var transportWeb = new Web(ApiKey);

            // Send the email.
            return transportWeb.DeliverAsync(myMessage);
        }        

        public string GetMailTemplate(string templateName)
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"MailTemplates\";
            var file = File.ReadAllText(filePath + templateName);

            return file;
        }

        //public List<AttachmentModel> ConvertAttachmentToStream(List<AttachmentRequestModel> attachments)
        //{
        //    var res = new List<AttachmentModel>();
        //    if (attachments == null) return res;
        //    foreach (var attachment in attachments)
        //    {
        //        var data = attachment.DataBase64String;
        //        if(string.IsNullOrEmpty(data) || string.IsNullOrEmpty(attachment.Name)) continue;
        //        var raw = Convert.FromBase64String(data);
        //        var stream = new MemoryStream(raw);
        //        res.Add(new AttachmentModel {Name = attachment.Name, Stream = stream});
        //    }
        //    return res;
        //}

        public List<string> GetEmailAddress(List<string> groups)
        {
            var groupName = groups.Select(x => "'"+x+"'").ToList();
            var listGroup = string.Join(",", groupName);
            var query = "select c.Email from ContactList c join GroupContact g on c.GroupContactId=g.Id and g.Name in("+ listGroup + ")";
            var emails = _context.Database.SqlQuery<string>(query).ToList();
            return emails;
        }
    }
}
