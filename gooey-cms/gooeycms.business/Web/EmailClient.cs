using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Gooeycms.Business.Web
{
    public class EmailClient
    {
        private String smtpServer;
        private String smtpPort;
        private String username;
        private String password;
        private String fromAddress;
        private String toAddress;
        private bool isHtmlContent;

        public virtual String ToAddress
        {
            get { return this.toAddress; }
            set { this.toAddress = value.Trim(); }
        }

        public virtual String FromAddress
        {
            get { return this.fromAddress; }
            set { this.fromAddress = value.Trim(); }
        }

        public virtual String Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public virtual String Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public virtual String SmtpServer
        {
            get { return this.smtpServer; }
            set { this.smtpServer = value; }
        }

        public virtual String SmtpPort
        {
            get { return this.smtpPort; }
            set { this.smtpPort = value; }
        }

        public virtual bool IsHtmlContent
        {
            get { return this.isHtmlContent; }
            set { this.isHtmlContent = value; }
        }

        public void Send(String subject, String message)
        {
            MailMessage mail = new MailMessage(this.fromAddress, this.toAddress);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = IsHtmlContent;

            int port = 25;
            Int32.TryParse(this.smtpPort, out port);

            SmtpClient client = new SmtpClient();
            client.Host = this.smtpServer;
            client.Port = port;
            if (this.username != null)
            {
                client.UseDefaultCredentials = false;
                NetworkCredential credentials = new NetworkCredential(this.username, this.password);
                client.Credentials = credentials;
            }

            client.Send(mail);
        }

        public void Send(String subject, StringBuilder message)
        {
            Send(subject, message.ToString());
        }

        public static EmailClient GetDefaultClient()
        {
            EmailClient client = new EmailClient();
            client.SmtpServer = GooeyConfigManager.SmtpServer;
            client.SmtpPort = GooeyConfigManager.SmtpPort;

            if (GooeyConfigManager.SmtpUsername != null)
            {
                client.Username = GooeyConfigManager.SmtpUsername;
                client.Password = GooeyConfigManager.SmtpPassword;
            }

            return client;
        }
    }
}

