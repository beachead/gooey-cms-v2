using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Web;
using Gooeycms.Business.Crypto;

namespace Gooeycms.Business.Subscription
{
    public class InviteManager
    {
        private static InviteManager instance = new InviteManager();
        public InviteManager() { }
        public static InviteManager Instance { get { return InviteManager.instance; } }

        public IList<CmsInvite> GetInvites()
        {
            CmsInviteDao dao = new CmsInviteDao();
            return dao.FindInvites();
        }

        public void Delete(Data.Guid guid)
        {
            CmsInviteDao dao = new CmsInviteDao();
            CmsInvite invite = dao.FindByGuid(guid);
            if (invite == null)
                throw new ArgumentException("Could not find an invite matching the specified guid: " + guid);

            //Invalidate the token if it was already issued
            if (!String.IsNullOrEmpty(invite.Token))
                TokenManager.Invalidate(invite.Token);

            using (Transaction tx = new Transaction())
            {
                dao.Delete<CmsInvite>(invite);
                tx.Commit();
            }
        }

        public void Approve(Data.Guid guid)
        {
            CmsInviteDao dao = new CmsInviteDao();
            CmsInvite invite = dao.FindByGuid(guid);
            if (invite == null)
                throw new ArgumentException("Could not find an invite matching the specified guid: " + guid);

            String token = TokenManager.Issue(guid.Value,TimeSpan.FromDays(60),1);

            invite.Issued = DateTime.Now;
            invite.Token = token;

            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsInvite>(invite);
                tx.Commit();
            }
            SendEmail(GooeyConfigManager.ApprovedEmailTemplate, "GooeyCMS Invite Request", invite);
        }

        public void Add(String firstname, String lastname, String email)
        {
            CmsInviteDao dao = new CmsInviteDao();
            CmsInvite invite = dao.FindByEmail(email);
            if (invite != null)
                throw new ArgumentException("This email address has already been registered.");

            invite = new CmsInvite();
            invite.Guid = System.Guid.NewGuid().ToString();
            invite.Firstname = firstname;
            invite.Lastname = lastname;
            invite.Email = email;
            invite.Created = DateTime.Now;
            invite.Issued = DateTime.MaxValue;
            invite.Responded = DateTime.MaxValue;

            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsInvite>(invite);
                tx.Commit();
            }

            SendEmail(GooeyConfigManager.InviteEmailTemplate, "GooeyCMS Invite Request", invite);
        }

        private void SendEmail(string template, String subject, CmsInvite invite)
        {
            template = template.Replace("{firstname}", invite.Firstname);
            template = template.Replace("{lastname}", invite.Lastname);
            template = template.Replace("{email}", invite.Email);
            template = template.Replace("{token}", invite.Token);

            EmailClient client = new EmailClient();
            client.SmtpServer = GooeyConfigManager.SmtpServer;
            client.SmtpPort = GooeyConfigManager.SmtpPort;
            client.ToAddress = invite.Email;
            client.FromAddress = "invites@gooeycms.com";
            client.IsHtmlContent = false;
            client.Send(subject, template);
        }
    }
}
