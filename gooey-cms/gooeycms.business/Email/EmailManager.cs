using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Subscription;
using System.Text.RegularExpressions;
using Gooeycms.Constants;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Email
{
    public class EmailManager
    {
        private static Regex SubjectPattern = new Regex(@"subject:\s*(.*)", RegexOptions.IgnoreCase);

        private static EmailManager instance = new EmailManager();
        private EmailManager() { }
        public static EmailManager Instance { get { return EmailManager.instance; } }

        private class EmailInfo
        {
            public String Subject { get; set; }
            public String Body { get; set; }
            public String To { get; set; }
            public String From { get; set; }
        }

        public void SendRegistrationEmail(CmsSubscription subscription)
        {
            String template = GooeyConfigManager.GetEmailTemplate(EmailTemplates.Signup);
            String body = PerformReplacements(subscription, template);

            EmailInfo info = GetEmailInfo(body, subscription);
            SendEmail(info);
        }

        private void SendEmail(EmailInfo info)
        {
            EmailClient client = EmailClient.GetDefaultClient();
            client.FromAddress = info.From;
            client.ToAddress = info.To;
            client.Send(info.Subject, info.Body);
        }

        private EmailInfo GetEmailInfo(String body, CmsSubscription subscription)
        {
            EmailInfo info = new EmailInfo();
            
            Match match = SubjectPattern.Match(body);
            if (match.Success)
            {
                info.Subject = match.Groups[1].Value;
                if (info.Subject != null)
                    info.Subject = info.Subject.Trim();

                body = SubjectPattern.Replace(body, "");
            }

            info.Body = body.Trim();
            info.To = subscription.PrimaryUser.Email;
            info.From = GooeyConfigManager.EmailAddresses.SiteAdmin;

            return info;
        }

        private String PerformReplacements(CmsSubscription subscription, String body)
        {
            Double totalCost = SubscriptionManager.CalculateCost(subscription);

            body = body.Replace("{email}", subscription.PrimaryUser.Email);
            body = body.Replace("{firstname}", subscription.PrimaryUser.Firstname);
            body = body.Replace("{username}", subscription.PrimaryUser.Username);
            body = body.Replace("{domain}", subscription.Domain);
            body = body.Replace("{staging}", subscription.StagingDomain);
            body = body.Replace("{subscription-cost}", String.Format("{0:c}",totalCost));
            body = body.Replace("{subscription-description}",SubscriptionManager.GetSubscriptionDescription(subscription).ToString());
            body = body.Replace("{paypal-id}", subscription.PaypalProfileId);
            body = body.Replace("{date}",DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));

            return body.Trim();
        }
    }
}
