using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LogItUpApi.Shared
{
    public class SendGridEmailSender
    {
        public async Task SendEmail(EmailInfo emailInfo)
        {
            var client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));

            var msg = MailHelper.CreateSingleEmail(
                        new EmailAddress(emailInfo.SenderEmailAddress, emailInfo.SenderName),
                        new EmailAddress(emailInfo.ReceiverEmailAddress, emailInfo.ReceiverName), 
                        emailInfo.Subject, 
                        emailInfo.PlainTextContent, 
                        emailInfo.HtmlContent);

            if(emailInfo.EmailAttachments != null)
            {
                foreach(var item in emailInfo.EmailAttachments)
                {
                    msg.AddAttachment(item.FileName, Convert.ToBase64String(item.File, 0, item.File.Length));
                }
            }

            var response = await client.SendEmailAsync(msg);
        }
    }

    public class EmailInfo
    {
        public string SenderEmailAddress { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string ReceiverEmailAddress { get; set; }
        public string ReceiverName { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }

        public IList<EmailAttachment> EmailAttachments { get; private set; }

        public void AddAttachment(string FileName, byte[] File)
        {
            if (EmailAttachments == null)
                EmailAttachments = new List<EmailAttachment>();

            EmailAttachments.Add(new EmailAttachment()
            {
                File = File,
                FileName = FileName
            });
        }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
