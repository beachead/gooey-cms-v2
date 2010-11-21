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

namespace Gooeycms.Business.Subscription
{
    public static class SubscriptionManager
    {
        public static List<String> InvalidSubdomainPrefixes = new List<String>() { "demo-","staging-" };

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
            subscription.Subdomain = registration.Sitename;
            
            if (String.IsNullOrEmpty(registration.Domain))
                subscription.Domain = registration.Sitename + GooeyConfigManager.DefaultCmsDomain;
            else
                subscription.Domain = registration.Domain;

            if (String.IsNullOrEmpty(registration.Staging))
                subscription.StagingDomain = GooeyConfigManager.DefaultStagingPrefix + registration.Sitename + GooeyConfigManager.DefaultCmsDomain;
            else
                subscription.StagingDomain = registration.Staging;

            subscription.SubscriptionPlanSku = registration.SubscriptionPlanSku;
            subscription.Expires = DateTime.Now.AddYears(100);
            subscription.IsDisabled = false;
            subscription.PrimaryUserGuid = wrapper.UserInfo.Guid;
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
            CmsTheme theme = ThemeManager.Instance.Add(subscription.Guid,"Gooey Default Theme", "A bare-bones default theme");
            theme.IsEnabled = true;
            ThemeManager.Instance.Save(theme);

            CmsTemplate template = new CmsTemplate();
            template.Content = GooeyConfigManager.DefaultTemplate;
            template.IsGlobalTemplateType = false;
            template.Name = "Gooey Default Page Template";
            template.SubscriptionGuid = subscription.Guid;
            template.LastSaved = DateTime.Now;
            template.Theme = theme;
            TemplateManager.Instance.Save(template);

            //Create a defaults for this site.
            SiteHelper.Configure(subscription.Guid);

            return subscription;
        }

        public static string GetDescription(Registration registration)
        {
            CmsSubscriptionPlan plan = GetSubscriptionPlan(registration);

            StringBuilder desc = new StringBuilder();
            desc.Append(plan.Name + " @ $" + plan.Price + " / month");
            if (registration.IsCampaignEnabled)
                desc.Append(" + Campaign Option @ $" + GooeyConfigManager.CampaignOptionPrice + " / month");
            if (registration.IsSalesforceEnabled)
                desc.Append(" + Salesforce Option @ $" + GooeyConfigManager.SalesForcePrice + " / month");

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

            return total;
        }

        public static IList<CmsSubscription> GetSubscriptionsByUserId(int userId)
        {

            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindByUserId(userId);
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
                CmsSubscriptionDao dao = new CmsSubscriptionDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsSubscription>(subscription);
                    tx.Commit();
                }

                Erase(subscription.Guid);
            }
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
    }
}
