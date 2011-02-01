using System;
using System.Collections.Generic;
using System.Text;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Membership;
using Gooeycms.Constants;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Web;
using Gooeycms.Business.Crypto;
using Gooeycms.Data.Model.Content;
using Gooeycms.Data.Model.Site;
using Gooeycms.Business.Paypal;
using Gooeycms.Business.Billing;
using Gooeycms.Business.Twilio;

namespace Gooeycms.Business.Subscription
{
    public static class SubscriptionManager
    {
        public static List<String> InvalidSubdomainPrefixes = new List<String>() { "demo-","staging-", "gooeycdn" };

        public static bool IsSubdomainValid(String subdomain)
        {
            return (!InvalidSubdomainPrefixes.Exists(d => subdomain.Contains(d)));
        }

        public static bool IsSubdomainAvailable(String subdomain)
        {
            bool result = false;

            bool validSubdomain = IsSubdomainValid(subdomain);
            if (validSubdomain)
            {
                CmsSubscriptionDao dao = new CmsSubscriptionDao();
                result = (dao.FindBySubdomain(subdomain) == null);
            }
            return result;
        }

        public static IList<CmsSubscriptionPlan> GetSubscriptionPlans()
        {
            CmsSubscriptionPlanDao dao = new CmsSubscriptionPlanDao();
            return dao.FindSubscriptionPlans();
        }

        public static CmsSubscriptionPlan GetSubscriptionPlan(Registration registration)
        {
            SubscriptionPlans temp = (SubscriptionPlans)Enum.Parse(typeof(SubscriptionPlans), registration.SubscriptionPlanSku, true);
            CmsSubscriptionPlan plan = SubscriptionManager.GetSubscriptionPlan(temp.ToString());
            return plan;
        }

        public static CmsSubscriptionPlan GetSubscriptionPlan(String sku)
        {
            CmsSubscriptionPlanDao dao = new CmsSubscriptionPlanDao();
            CmsSubscriptionPlan cmsPlan = dao.FindBySku(sku);

            return cmsPlan;
        }

        public static CmsSubscriptionPlan GetSubscriptionPlan(SubscriptionPlans plan)
        {
            return GetSubscriptionPlan(plan.ToString());
        }

        public static CmsSubscription CreateFromRegistration(Registration registration)
        {
            MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(registration.Email);
            if (!wrapper.IsValid())
                throw new ApplicationException("The registration has not been properly saved to the database.");

            CmsSubscription subscription = new CmsSubscription();
            subscription.Guid = registration.Guid;
            subscription.Created = DateTime.Now;
            subscription.Culture = "en-us";
            subscription.Subdomain = registration.Sitename;
            
            if (String.IsNullOrEmpty(registration.Domain))
                subscription.Domain = registration.Sitename + GooeyConfigManager.DefaultCmsDomain;
            else
                subscription.Domain = registration.Domain;

            if (String.IsNullOrEmpty(registration.Staging))
                subscription.StagingDomain = GooeyConfigManager.DefaultStagingPrefix + registration.Sitename + GooeyConfigManager.DefaultCmsDomain;
            else
                subscription.StagingDomain = registration.Staging;

            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(registration);
            subscription.Expires = DateTime.Now.AddYears(100);
            subscription.IsDisabled = false;
            subscription.PrimaryUser = wrapper.UserInfo;
            subscription.IsSalesforceEnabled = registration.IsSalesforceEnabled;
            subscription.IsCampaignEnabled = registration.IsCampaignEnabled;
            subscription.IsGenericOptionsEnabled = registration.IsGenericOptionEnabled;

            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            using (Transaction tx = new Transaction())
            {
                dao.SaveObject(subscription);
                tx.Commit();
            }

            using (Transaction tx = new Transaction())
            {
                dao.AddUserToSubscription(wrapper.UserInfo.Id, subscription.Id);
                tx.Commit();
            }

            //Create a default template for this site.
            CmsTheme theme = ThemeManager.Instance.Add(subscription.Guid,GooeyConfigManager.DefaultThemeName, GooeyConfigManager.DefaultThemeDescription);
            theme.IsEnabled = true;
            ThemeManager.Instance.Save(theme);

            CmsTemplate template = new CmsTemplate();
            template.Content = GooeyConfigManager.DefaultTemplate;
            template.IsGlobalTemplateType = false;
            template.Name = GooeyConfigManager.DefaultTemplateName;
            template.SubscriptionGuid = subscription.Guid;
            template.LastSaved = DateTime.Now;
            template.Theme = theme;
            TemplateManager.Instance.Save(template);

            //Create a defaults for this site.
            SiteHelper.Configure(subscription.Guid);

            return subscription;
        }

        public static void Save(CmsSubscription subscription)
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsSubscription>(subscription);
                tx.Commit();
            }
        }

        public static string GetDescription(Registration registration)
        {
            CmsSubscriptionPlan plan = GetSubscriptionPlan(registration);

            double total = (double)plan.Price;
            StringBuilder desc = new StringBuilder();
            desc.Append("<b>" + plan.Name + "</b> @ $" + plan.Price + " / month<br />");
            if (registration.IsCampaignEnabled)
            {
                desc.Append("<b>Campaign Option</b> @ " + String.Format("{0:c}", GooeyConfigManager.CampaignOptionPrice) + " / month<br />");
                total += GooeyConfigManager.CampaignOptionPrice;
            }
            if (registration.IsSalesforceEnabled)
            {
                desc.Append("<b>Salesforce Option</b> @ " + String.Format("{0:c}", GooeyConfigManager.SalesForcePrice + " / month<br />"));
                total += GooeyConfigManager.SalesForcePrice;
            }
            desc.Append("<br /><b>Order Total:</b> " + String.Format("{0:c}", total) + " / month<br />");
          

            return desc.ToString();
        }

        public static string GetShortDescription(String header, Registration registration)
        {
            CmsSubscriptionPlan plan = GetSubscriptionPlan(registration);

            StringBuilder desc = new StringBuilder();
            desc.Append(header + " " + plan.Name);
            if (registration.IsCampaignEnabled)
                desc.Append(" + Campaigns");
            if (registration.IsSalesforceEnabled)
                desc.Append(" + (incl. Salesforce)"); 

            return desc.ToString();
        }

        public static Double CalculateCost(Registration registration)
        {
            CmsSubscriptionPlan plan = GetSubscriptionPlan(registration);

            Double total = (double)plan.Price;
            if (registration.IsSalesforceEnabled)
                total += GooeyConfigManager.SalesForcePrice;
            if (registration.IsCampaignEnabled)
                total += GooeyConfigManager.CampaignOptionPrice;

            return total;
        }

        public static Double CalculateCost(CmsSubscription subscription)
        {
            CmsSubscriptionPlan plan = subscription.SubscriptionPlan;

            Double total = (double)plan.Price;
            if (subscription.IsSalesforceEnabled)
                total += GooeyConfigManager.SalesForcePrice;
            if (subscription.IsCampaignEnabled)
                total += GooeyConfigManager.CampaignOptionPrice;

            return total;
        }

        public static IList<CmsSubscriptionPhoneNumber> GetActivePhoneNumbers(Data.Guid siteGuid)
        {
            CmsSubscriptionPhoneDao dao = new CmsSubscriptionPhoneDao();
            return dao.FindBySiteGuid(siteGuid);
        }

        public static IList<CmsSubscription> GetSubscriptionsByUserId(int userId)
        {

            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindByUserId(userId);
        }

        public static IList<CmsSubscription> GetAllSubscriptions()
        {
            SubscriptionAdapter adapter = new SubscriptionAdapter();
            return adapter.GetAllSubscriptions();
        }

        public static CmsSubscription GetSubscription(Data.Guid siteGuid)
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindByGuid(siteGuid);
        }

        public static CmsSubscription GetSubscriptionForDomain(string host)
        {
            String subdomain = host.ToLower().Replace(GooeyConfigManager.DefaultCmsDomain,"");
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindByDomains(subdomain, host);
        }

        public static void RemoveUserFromSubscription(CmsSubscription subscription, UserInfo user)
        {
            if (subscription != null)
            {
                CmsSubscriptionDao dao = new CmsSubscriptionDao();
                using (Transaction tx = new Transaction())
                {
                    dao.RemoveUserFromSubscription(user.Id, subscription.Id);
                    tx.Commit();
                }

                //Check if this user is associated to any other subscriptions, if not, clean up the membership system
                IList<CmsSubscription> subscriptions = GetSubscriptionsByUserId(user.Id);
                if (subscriptions.Count == 0)
                    MembershipUtil.DeleteUser(user);
            }
        }

        public static void AddUserToSubscription(Data.Guid siteGuid, UserInfo user)
        {
            CmsSubscription subscription = GetSubscription(siteGuid);
            if (subscription != null)
            {
                CmsSubscriptionDao dao = new CmsSubscriptionDao();
                using (Transaction tx = new Transaction())
                {
                    dao.AddUserToSubscription(user.Id, subscription.Id);
                    tx.Commit();
                }
            }
        }

        public static void Create(MembershipUserWrapper wrapper, CmsSubscription subscription)
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            using (Transaction tx = new Transaction())
            {
                dao.SaveObject(subscription);
                tx.Commit();
            }

            using (Transaction tx = new Transaction())
            {
                dao.AddUserToSubscription(wrapper.UserInfo.Id, subscription.Id);
                tx.Commit();
            }
        }

        internal static void Delete(CmsSubscription subscription)
        {
            if (subscription != null)
            {
                RemoveAllPhoneNumbers(subscription);

                CmsSubscriptionDao dao = new CmsSubscriptionDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsSubscription>(subscription);
                    tx.Commit();
                }

                Erase(subscription.Guid);
            }
        }

        internal static void RemoveAllPhoneNumbers(CmsSubscription subscription)
        {
            //Find any phone numbers and release them
            IList<CmsSubscriptionPhoneNumber> numbers = GetActivePhoneNumbers(subscription.Guid);
            foreach (CmsSubscriptionPhoneNumber number in numbers)
                SubscriptionManager.RemovePhoneFromSubscription(subscription.Guid, number.PhoneNumber);
        }

        /// <summary>
        /// Erases all of the data for a site
        /// </summary>
        /// <param name="siteGuid"></param>
        public static void Erase(Data.Guid siteGuid)
        {
            using (Transaction tx = new Transaction())
            {
                //Delete the cms content
                CmsContentDao dao = new CmsContentDao();
                dao.DeleteAllBySite(siteGuid);

                //Delete the cms content types
                CmsContentTypeDao typeDao = new CmsContentTypeDao();
                typeDao.DeleteAllBySite(siteGuid);

                //Delete the pages
                CmsPageDao pageDao = new CmsPageDao();
                pageDao.DeleteAllBySite(siteGuid);

                //Delete the sitemap data for this subscription
                CmsSitePathDao siteDao = new CmsSitePathDao();
                siteDao.DeleteAllBySite(siteGuid);

                //Delete the templates
                CmsTemplateDao templateDao = new CmsTemplateDao();
                templateDao.DeleteAllBySite(siteGuid);

                //Delete the existing themes
                CmsThemeDao themeDao = new CmsThemeDao();
                themeDao.DeleteAllBySite(siteGuid);

                tx.Commit();
            }

            //Delete any cloud storage files for this subscription
            String pagesContainer = String.Format(SiteHelper.PageContainerKey, siteGuid);
            String javascriptContainer = String.Format(SiteHelper.JavascriptContainerKey, siteGuid);
            String cssContainer = String.Format(SiteHelper.StylesheetContainerKey, siteGuid);
            String imagesContainer = String.Format(SiteHelper.ImagesContainerKey, siteGuid);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.Delete(pagesContainer);
            client.Delete(javascriptContainer);
            client.Delete(cssContainer);
            client.Delete(imagesContainer);
        }

        public static Boolean EnableSubscription(CmsSubscription subscription)
        {
            subscription.IsDisabled = false;
            Save(subscription);

            //Reactivate the billing within paypal
            PaypalExpressCheckout action = new PaypalExpressCheckout();
            return action.Reactivate(subscription.PaypalProfileId);
        }

        public static Boolean DisableSubscription(CmsSubscription subscription)
        {
            subscription.IsDisabled = true;
            Save(subscription);

            //Remove any phone numbers associated with this subscription
            RemoveAllPhoneNumbers(subscription);

            //Disable the billing within paypal
            PaypalExpressCheckout action = new PaypalExpressCheckout();
            PaypalProfileInfo info = action.GetProfileInfo(subscription.PaypalProfileId);

            Boolean result = true;
            if ((info != null) && (info.IsActive))
                result = action.Suspend(subscription.PaypalProfileId);

            return result;
        }

        public static void CancelSubscription(CmsSubscription subscription)
        {
            PaypalExpressCheckout checkout = new PaypalExpressCheckout();
            PaypalProfileInfo info = checkout.GetProfileInfo(subscription.PaypalProfileId);
            if (info != null)
            {
                if (info.ProfileStatus != PaypalProfileInfo.ProfileStatusEnum.Cancelled)
                    checkout.Cancel(subscription.PaypalProfileId);
            }

            try
            {
                //Remove the users from this site
                IList<UserInfo> users = MembershipUtil.GetUsersBySite(subscription.Id);
                foreach (UserInfo user in users)
                {
                    SubscriptionManager.RemoveUserFromSubscription(subscription,user);
                }

                SubscriptionManager.Delete(subscription);
            }
            catch (Exception e)
            {
                CmsSubscription stillexists = GetSubscription(subscription.Guid);
                if (stillexists != null)
                {
                    stillexists.IsDisabled = true;
                    Save(stillexists);
                }
                throw e;
            }
        }

        public static void ExtendTrialPeriod(CmsSubscription subscription, Int32 numberOfCyclesToAdd)
        {
            PaypalExpressCheckout checkout = new PaypalExpressCheckout();
            PaypalProfileInfo info = checkout.GetProfileInfo(subscription.PaypalProfileId);
            if (info != null)
            {
                if (!info.IsTrialPeriod)
                    throw new PaypalException("This subscription's trial period can not be extended. Reason: Trial period has already expired");

                int numberOfCycles = info.TrialCyclesRemaining + numberOfCyclesToAdd;
                checkout.ExtendTrialPeriod(subscription.PaypalProfileId, numberOfCycles);
            }
        }

        public static void UpdateBillingAgreement(double originalCost, CmsSubscription subscription)
        {
            //Check if the subscription plan is now free, if so, cancel the paypal profile
            PaypalExpressCheckout checkout = new PaypalExpressCheckout();

            if (subscription.SubscriptionPlan.Price <= 0)
            {
                checkout.Cancel(subscription.PaypalProfileId);
                BillingManager.Instance.AddHistory(subscription.Guid, subscription.PaypalProfileId, BillingManager.NotApplicable, BillingManager.SubscriptionModification, 0d, "Gooeycms administrator modified subscription plan to " + subscription.SubscriptionPlan.Name);
            }
            else
            {
                Double newCost = CalculateCost(subscription);

                //If it's more, than update the billing agreement
                if (originalCost > newCost)
                {
                    StringBuilder description = GetSubscriptionDescription(subscription);

                    String desc = description.ToString();
                    checkout.UpdateBillingAgreement(subscription.PaypalProfileId, newCost, desc);

                    BillingManager.Instance.AddHistory(subscription.Guid, subscription.PaypalProfileId, BillingManager.NotApplicable, BillingManager.SubscriptionModification, newCost, "Gooeycms administrator modified subscription: " + desc);
                }
                else
                {
                    BillingManager.Instance.AddHistory(subscription.Guid, subscription.PaypalProfileId, BillingManager.NotApplicable, BillingManager.SubscriptionModification, originalCost, "Gooeycms administrator modified subscription plan options. Salesforce enabled: " + subscription.IsSalesforceEnabled + ", Campaigns enabled: " + subscription.IsCampaignEnabled);
                }
            }

            //If there weren't any problems update the subscription in our database
            Save(subscription);
        }

        public static StringBuilder GetSubscriptionDescription(CmsSubscription subscription)
        {
            StringBuilder description = new StringBuilder();
            description.AppendFormat("{0} / {1:c} ", subscription.SubscriptionPlan.Name, subscription.SubscriptionPlan.Price);
            if (subscription.IsCampaignEnabled)
                description.AppendFormat(" +Campaigns / {0:c} ", GooeyConfigManager.CampaignOptionPrice);

            if (subscription.IsSalesforceEnabled)
                description.AppendFormat(" +Salesforce / {0:c} ", GooeyConfigManager.SalesForcePrice);

            double cost = CalculateCost(subscription);
            description.AppendFormat(". Total: {0:c} / month.", cost);
            return description;
        }

        public static void UpdateDomains(CmsSubscription subscription, String productionDomain, String stagingDomain, bool validateDomain)
        {
            //Validate that the domains are valid
            if (validateDomain)
            {
                ValidateDomain(subscription.Subdomain,productionDomain);

                String stagingAssignedName = GooeyConfigManager.DefaultStagingPrefix + subscription.Subdomain;
                ValidateDomain(stagingAssignedName, stagingDomain);
            }

            subscription.Domain = productionDomain;
            subscription.StagingDomain = stagingDomain;

            Save(subscription);
        }

        public static CmsSubscription GetSubscriptionByProfileId(string profileId)
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindByProfileId(profileId);
        }

        public static void ValidateDomain(String assignedName, String domain)
        {
            
            if (String.IsNullOrEmpty(domain))
                throw new ArgumentException("You must specify a domain");

            int pos = domain.IndexOf('.');
            String subdomain = domain.Substring(0, pos);
            String assignedDomain = assignedName + GooeyConfigManager.DefaultCmsDomain;

            //Make sure that if they're using the gooey default domain, that it matches their assigned name
            if (domain.ToLower().EndsWith(GooeyConfigManager.DefaultCmsDomain))
            {
                if (!subdomain.Equals(assignedName, StringComparison.CurrentCultureIgnoreCase))
                    throw new ArgumentException("The domain " + domain + " is not valid. You must use your assigned domain " + assignedDomain + " or a custom domain.");
            }

            //Make sure the domain is actually valid
            Boolean result = Uri.IsWellFormedUriString("http://" + domain,UriKind.Absolute);
            if (!result)
                throw new ArgumentException("The domain " + domain + " is not a valid domain and may not be used.");
        }

        public static void UpdateSubscriptionOptions(CmsSubscription subscription, bool campaignsEnabled, bool salesforceEnabled)
        {
            Double currentCost = CalculateCost(subscription);

            //Get the new cost
            subscription.IsCampaignEnabled = campaignsEnabled;
            subscription.IsSalesforceEnabled = salesforceEnabled;
            Double newCost = CalculateCost(subscription);

            String newDescription = GetSubscriptionDescription(subscription).ToString();
            PaypalExpressCheckout checkout = new PaypalExpressCheckout();

            PaypalProfileInfo info = checkout.GetProfileInfo(subscription.PaypalProfileId);
            //If the cost is less, we can just update Paypal directly. So do so
            if (newCost < currentCost)
            {
                if (info.IsTrialPeriod)
                    throw new ArgumentException("Due to Paypal billing limitations you may not remove options associated with this subscription during your 30-day trial period."); 

                checkout.UpdateBillingAgreement(subscription.PaypalProfileId, newCost, newDescription);
            }
            else
            {
                //We need to send the user to paypal to confirm the billing agreement. So, first, cancel the current agreement
                String returnurl = "http://" + GooeyConfigManager.AdminSiteHost + "/auth/Manage.aspx";
                String cancelurl = returnurl;

                checkout.Cancel(subscription.PaypalProfileId);
                BillingManager.Instance.AddHistory(subscription.Guid, subscription.PaypalProfileId, null, BillingManager.Cancel, 0, "Successfully cancelled existing paypal billing profile, will re-establish billing profile at a new rate of " + String.Format("{0:c}",newCost));

                checkout.AddBillingAgreement(PaypalExpressCheckout.GetBillingAgreement(subscription));
                String redirect = checkout.SetExpressCheckout(LoggedInUser.Email, subscription.Guid, returnurl, cancelurl);

                WebRequestContext.Instance.CurrentHttpContext.Response.Redirect(redirect, true);
            }
        }

        public static void AddPhoneToSubscription(CmsSubscriptionPhoneNumber phone)
        {
            if (phone.SubscriptionId == null)
                throw new ArgumentException("You must set the subscription guid prior to calling this method");

            CmsSubscriptionPhoneDao dao = new CmsSubscriptionPhoneDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsSubscriptionPhoneNumber>(phone);
                tx.Commit();
            }
        }

        public static CmsSubscriptionPhoneNumber GetPhoneNumber(String phone)
        {
            CmsSubscriptionPhoneDao dao = new CmsSubscriptionPhoneDao();
            return dao.FindByPhoneNumber(phone);
        }

        public static void RemovePhoneFromSubscription(Data.Guid guid, String phoneNumber)
        {
            if (!String.IsNullOrEmpty(phoneNumber))
            {
                CmsSubscriptionPhoneNumber phone = SubscriptionManager.GetPhoneNumber(phoneNumber);
                if (phone != null)
                    CurrentSite.Configuration.PhoneSettings.GetLocalTwilioClient().ReleasePhoneNumber(phone.Sid, AvailablePhoneNumber.Parse(phoneNumber));

                CmsSubscriptionPhoneDao dao = new CmsSubscriptionPhoneDao();
                if (!phone.SubscriptionId.Equals(guid.Value))
                    throw new ArgumentException("This phone number does not belong to this subscription");

                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsSubscriptionPhoneNumber>(phone);
                    tx.Commit();
                }
            }
        }

        public static double CalculateFreeTrialRemaining(CmsSubscription cmsSubscription)
        {
            int freeTrialDays = GooeyConfigManager.FreeTrialLength;
            DateTime startDate = cmsSubscription.Created;
            DateTime freeTrialExpires = startDate.AddDays(freeTrialDays);

            return freeTrialExpires.Subtract(DateTime.Now).TotalDays;
        }
    }
}
