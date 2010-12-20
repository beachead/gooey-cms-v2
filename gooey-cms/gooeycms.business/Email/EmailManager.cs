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
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business.Crypto;

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

        public void SendRegistrationEmail(CmsSubscription subscription, Registration registration)
        {
            String template = GooeyConfigManager.GetEmailTemplate(EmailTemplates.Signup);
            String body = PerformReplacements(template,subscription: subscription, registration: registration);

            EmailInfo info = GetEmailInfo(body, subscription.PrimaryUser.Email);
            SendEmail(info);
        }

        public void SendCancellationEmail(CmsSubscription subscription)
        {
            String template = GooeyConfigManager.GetEmailTemplate(EmailTemplates.Cancel);
            String body = PerformReplacements(template,subscription);

            EmailInfo info = GetEmailInfo(body, subscription.PrimaryUser.Email);
            SendEmail(info);
        }

        public void SendPurchaseEmail(Receipt receipt)
        {
            String template = GooeyConfigManager.GetEmailTemplate(EmailTemplates.Purchase);
            String body = PerformReplacements(template, receipt: receipt);

            UserInfo user = MembershipUtil.FindByUserGuid(receipt.UserGuid).UserInfo;
            EmailInfo info = GetEmailInfo(body,user.Email);
            SendEmail(info);
        }

        private void SendEmail(EmailInfo info)
        {
            EmailClient client = EmailClient.GetDefaultClient();
            client.FromAddress = info.From;
            client.ToAddress = info.To;
            client.Send(info.Subject, info.Body);
        }

        private EmailInfo GetEmailInfo(String body, String emailTo)
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
            info.To = emailTo;
            info.From = GooeyConfigManager.EmailAddresses.SiteAdmin;

            return info;
        }

        private String PerformReplacements(String body, CmsSubscription subscription = null, Registration registration = null, Receipt receipt = null)
        {
            UserInfo user = null;
            if (receipt == null)
                user = MembershipUtil.FindByUserGuid(subscription.PrimaryUserGuid).UserInfo;
            else
                user = MembershipUtil.FindByUserGuid(receipt.UserGuid).UserInfo;

            body = body.Replace("{email}", user.Email);
            body = body.Replace("{firstname}", user.Firstname);
            body = body.Replace("{lastname}", user.Lastname);
            body = body.Replace("{username}", user.Username);
            body = body.Replace("{current-date}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));

            if (subscription != null)
            {
                Double totalCost = SubscriptionManager.CalculateCost(subscription);
                body = body.Replace("{domain}", subscription.Domain);
                body = body.Replace("{staging-domain}", subscription.StagingDomain);
                body = body.Replace("{subscription-cost}", String.Format("{0:c}", totalCost));
                body = body.Replace("{subscription-description}", SubscriptionManager.GetSubscriptionDescription(subscription).ToString());
                body = body.Replace("{paypal-id}", subscription.PaypalProfileId);
            }
            else
            {
                body = body.Replace("{domain}", String.Empty);
                body = body.Replace("{staging}", String.Empty);
                body = body.Replace("{subscription-cost}", String.Empty);
                body = body.Replace("{subscription-description}", String.Empty);
                body = body.Replace("{paypal-id}", String.Empty);
            }

            if (registration != null)
            {
                String password = TextEncryption.Decode(registration.EncryptedPassword);
                body = body.Replace("{password}", password);
            }
            else
            {
                body = body.Replace("{password}", String.Empty);
            }

            if (receipt != null)
            {
                String packageGuid = receipt.PackageGuid;
                Package package = SitePackageManager.NewInstance.GetPackage(packageGuid);

                body = body.Replace("{purchase-type}", package.PackageType.ToString());
                body = body.Replace("{purchase-date}", receipt.Created.ToString("MM/dd/yyyy"));
                body = body.Replace("{purchase-amount}", String.Format("{0:c}",receipt.Amount));
                body = body.Replace("{purchase-name}", package.Title);
                body = body.Replace("{purchase-txid}", receipt.TransactionId);
            }

            return body.Trim();
        }
    }
}
