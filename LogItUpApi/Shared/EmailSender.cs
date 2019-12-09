using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LogItUpApi.Shared.EmailSender
{
    public interface IEmailSender
    {
        public void SendEmail(EmailConfig emailConfig, EmailInfo emailInfo);
    }

    public class EmailSender : IEmailSender
    {
        public void SendEmail(EmailConfig emailConfig, EmailInfo emailInfo)
        {
            MailMessage mailMessage = GetMailMessage(emailInfo);

            SmtpClient smtpClient = GetSmtpClient(emailConfig);

            smtpClient.Send(mailMessage);
        }

        private SmtpClient GetSmtpClient(EmailConfig emailConfig)
        {
            SmtpClient smtp = new SmtpClient(emailConfig.SmtpServer, emailConfig.SmtpPort);

            smtp.Credentials = new System.Net.NetworkCredential(emailConfig.SenderAddress, emailConfig.SenderPassword);

            smtp.EnableSsl = true;

            smtp.Timeout = 60000;

            return smtp;
        }

        private MailMessage GetMailMessage(EmailInfo emailInfo)
        {
            MailAddress addressFrom = new MailAddress(emailInfo.SenderAddress);

            MailAddress addressTo = new MailAddress(emailInfo.ReceiverAddress);

            MailMessage message = new MailMessage(addressFrom, addressTo)
            {
                Subject = emailInfo.Subject,
                Body = emailInfo.Body
            };

            if (emailInfo.EmailAttachment != null)
            {
                foreach (var item in emailInfo.EmailAttachment)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(item.File), item.Name));
                }
            }

            message.IsBodyHtml = true;

            return message;
        }
    }

    public class EmailConfig
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPassword { get; set; }
    }
    public class EmailInfo
    {
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<EmailAttachment> EmailAttachment { get; set; }
    }
    public class EmailAttachment
    {
        public string Name { get; set; }
        public byte[] File { get; set; }
    }
}
