using System;
using System.Collections.Generic;
using Gooeycms.Business.Cache;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Data.Model;
using Gooeycms.Extensions;
using System.Web.UI;
using Gooeycms.Business.Campaigns;
using Gooeycms.Business.Campaigns.Engine;
using Beachead.Persistence.Hibernate;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Crypto;
using Gooeycms.Constants;
using System.Text;

namespace Gooeycms.Business.Util
{
    public static class CurrentSite
    {
        public static CacheInstance Cache
        {
            get
            {
                return CacheManager.Instance.GetCache(CurrentSite.Guid);
            }
        }

        public static CmsSubscription Subscription
        {
            get
            {
                CmsSubscription result = Cache.Get<CmsSubscription>("subscription");
                if ((result == null) || (GooeyConfigManager.IsDevelopmentEnvironment))
                {
                    result = SubscriptionManager.GetSubscription(CurrentSite.Guid);
                    Cache.Add("subscription", result);
                }
                return result;
            }
        }

        public static CmsSubscriptionPlan SubscriptionPlan
        {
            get
            {
                CmsSubscriptionPlan plan = Cache.Get<CmsSubscriptionPlan>("subscription-plan");
                if ((plan == null) || (GooeyConfigManager.IsDevelopmentEnvironment))
                {
                    plan = SubscriptionManager.GetSubscriptionPlan(Subscription.SubscriptionPlanEnum);
                    Cache.Add("subscription-plan",plan);
                }
                return plan;
            }
        }

        public static class Configuration
        {
            private static void SetEncryptedSiteConfiguration(String key, String value)
            {
                TextEncryption crypto = new TextEncryption(CurrentSite.Guid.Value);
                String encryptedValue = crypto.Encrypt(value);

                SetSiteConfiguration(key, encryptedValue);
            }

            private static void SetSiteConfiguration(String key, String value)
            {
                Data.Guid guid = CurrentSite.Guid;

                SiteConfigurationDao dao = new SiteConfigurationDao();
                SiteConfiguration result = dao.FindByKey(guid, key);
                if (result == null)
                    result = new SiteConfiguration();

                result.Name = key;
                result.Value = value;
                result.SubscriptionGuid = guid.Value;
                using (Transaction tx = new Transaction())
                {
                    dao.Save<SiteConfiguration>(result);
                    tx.Commit();
                }

                Cache.Clear(key);
                Cache.Add(key, value);
            }

            private static String GetEncryptedSiteConfiguration(String key)
            {
                String result = null;
                String encrypted = GetSiteConfiguration(key, null, false);
                if (encrypted != null)
                {
                    try
                    {
                        TextEncryption crypto = new TextEncryption(CurrentSite.Guid.Value);
                        result = crypto.Decrypt(encrypted);
                    }
                    catch (Exception) { }
                }

                return result;
            }

            private static String GetSiteConfiguration(String key, String def, Boolean required)
            {
                String value = Cache.Get<String>(key);
                if (value == null)
                {
                    Data.Guid guid = CurrentSite.Guid;
                    SiteConfigurationDao dao = new SiteConfigurationDao();

                    Gooeycms.Data.Model.SiteConfiguration result = dao.FindByKey(guid, key);
                    value = def;
                    if (result != null)
                        value = result.Value;

                    if ((required) && (String.IsNullOrEmpty(value)))
                        throw new ArgumentNullException("The configuration value for the site:" + guid + " and key " + key + " has not been set. This value is required for the site to function properly.");

                    Cache.Add(key, value);
                }
                return value;

            }

            public static Boolean IsGoogleAnalyticsEnabled
            {
                get 
                {
                    Boolean result = false;
                    String strResult = GetSiteConfiguration("google-analytics-enabled", "false", true);
                    Boolean.TryParse(strResult, out result);

                    return result;
                }
                set { SetSiteConfiguration("google-analytics-enabled", value.StringValue()); }
            }

            public static String GoogleAccountId
            {
                get { return GetSiteConfiguration("google-account-id", "", false); }
                set { SetSiteConfiguration("google-account-id", value); }
            }

            public static String HeaderImageTemplate
            {
                get { return GetSiteConfiguration("markup-headerimage", "", false); }
            }

            public static String LeadEmailFromAddress
            {
                get { return GetSiteConfiguration("lead-email-from", "form-leads@gooeycms.net", false); }
            }

            public static String LeadEmailSubject
            {
                get { return GetSiteConfiguration("lead-email-subject", "Lead Submission", false); }
            }

            public static Boolean IsLeadEmailEnabled
            {
                get
                {
                    String value = GetSiteConfiguration("lead-email-enabled", "true", false);
                    return Boolean.Parse(value);
                }
            }

            public static String MarkupEngineName
            {
                get
                {
                    String result = GetSiteConfiguration("markup-engine-name", null, false);
                    return result;
                }
            }

            public static class Salesforce
            {
                public static String Username
                {
                    get { return Configuration.GetEncryptedSiteConfiguration("salesforce-username"); }
                    set { Configuration.SetEncryptedSiteConfiguration("salesforce-username", value); }
                }

                public static String Password
                {
                    get { return Configuration.GetEncryptedSiteConfiguration("salesforce-password"); }
                    set { Configuration.SetEncryptedSiteConfiguration("salesforce-password", value); }
                }

                public static String Token
                {
                    get { return Configuration.GetEncryptedSiteConfiguration("salesforce-token"); }
                    set { Configuration.SetEncryptedSiteConfiguration("salesforce-token", value); }
                }

                public static Boolean IsEnabled
                {
                    get { return Boolean.Parse(Configuration.GetSiteConfiguration("salesforce-enabled", "false", false)); }
                    set { Configuration.SetSiteConfiguration("salesforce-enabled", value.StringValue()); }
                }

                public static void AddCustomFieldMapping(String original, String custom)
                {
                    IDictionary<String, String> mappings = CustomFieldMappings;
                    StringBuilder builder = new StringBuilder();
                    foreach (String mapped in mappings.Keys)
                    {
                        builder.Append(mapped).Append("=").Append(mappings[mapped]).Append(TextConstants.DefaultSeparator);
                    }
                    builder.Append(custom).Append("=").Append(original);

                    String flattened = builder.ToString();
                    Configuration.SetSiteConfiguration("salesforce-mappings", flattened);
                }

                public static void RemoveCustomFieldMapping(String custom)
                {
                    IDictionary<String, String> mappings = CustomFieldMappings;
                    StringBuilder builder = new StringBuilder();
                    foreach (String mapped in mappings.Keys)
                    {
                        if (!mapped.Equals(custom))
                            builder.Append(mapped).Append("=").Append(mappings[mapped]).Append(TextConstants.DefaultSeparator);
                    }

                    String flattened = builder.ToString();
                    Configuration.SetSiteConfiguration("salesforce-mappings", flattened);
                }

                public static IDictionary<String, String> CustomFieldMappings
                {
                    get 
                    {
                        IDictionary<String, String> results = new Dictionary<String, String>(StringComparer.InvariantCultureIgnoreCase);
                        String result = Configuration.GetSiteConfiguration("salesforce-mappings", null, false);
                        if (result != null)
                        {
                            String[] pairs = result.Split(TextConstants.DefaultSeparator);
                            foreach (String pair in pairs)
                            {
                                String[] arr = pair.Split('=');
                                if (arr.Length == 2)
                                {
                                    results[arr[0].Trim()] = arr[1].Trim();
                                }
                            }
                        }
                        return results;
                    }
                }
            }
        }

        public class Restrictions
        {
            public static Int32 MaxAllowedPages
            {
                get { return CurrentSite.SubscriptionPlan.MaxAllowedPages; }
            }

            public static Boolean IsJavascriptAllowed
            {
                get { return CurrentSite.SubscriptionPlan.IsJavascriptAllowed; }
            }
        }

        private static String GetStorageKey(String type)
        {
            return String.Format(type, SiteHelper.GetActiveSiteGuid(true).Value);
        }

        public static String PageStorageContainer
        {
            get { return GetStorageKey(SiteHelper.PageContainerKey); }
        }

        public static String JavascriptStorageContainer
        {
            get { return GetStorageKey(SiteHelper.JavascriptContainerKey); }
        }

        public static String StylesheetStorageContainer
        {
            get { return GetStorageKey(SiteHelper.StylesheetContainerKey); }
        }

        public static String ImageStorageDirectory
        {
            get { return GetStorageKey(SiteHelper.ImagesContainerKey); }
        }

        public static Boolean IsSet
        {
            get
            {
                Boolean result = false;
                try
                {
                    SiteHelper.GetActiveSiteGuid(true);
                    result = true;
                }
                catch (Exception) { }

                return result;
            }
        }

        public static Data.Guid Guid
        {
            get { return SiteHelper.GetActiveSiteGuid(true); }
        }

        public static String ProductionDomain
        {
            get 
            {
                String result = CurrentSite.Cache.Get<String>("productiondomain");
                if (result == null)
                {
                    result = SiteHelper.GetProductionDomain(CurrentSite.Guid); 
                    CurrentSite.Cache.Add("productiondomain",result);
                }
                return result;
            }
        }

        public static String StagingDomain
        {
            get 
            {
                String result = CurrentSite.Cache.Get<String>("stagingdomain");
                if (result == null)
                {
                    result = SiteHelper.GetStagingDomain(CurrentSite.Guid); 
                    CurrentSite.Cache.Add("stagingdomain", result);
                }
                return result;
            }
        }

        public static ICampaignEngine GetCampaignEngine()
        {
            ICampaignEngine engine = CurrentSite.Cache.Get<ICampaignEngine>("campaign-engine");
            if (engine == null)
            {
                engine = new GoogleAnalytics();
                CurrentSite.Cache.Add("campaign-engine", engine);
            }

            return engine;
        }

        public static CmsTheme GetCurrentTheme()
        {
            CmsTheme result = CurrentSite.Cache.Get<CmsTheme>("currenttheme");
            if (result == null)
            {
                result = ThemeManager.Instance.GetDefaultBySite(Guid);
                CurrentSite.Cache.Add("currenttheme", result);
            }
            return result;
        }

        public static Boolean IsStagingHost
        {
            get
            {
                WebRequestContext context = new WebRequestContext();
                String domain = context.Request.Url.Host;

                Boolean result = false;
                //check if we're on the staging site
                if (Extensions.Extensions.EqualsCaseInsensitive(StagingDomain, domain))
                    result = true;
                else if (Extensions.Extensions.EqualsCaseInsensitive(ProductionDomain, domain))
                    result = false;
                else if (LoggedInUser.IsLoggedIn)
                {
                    result = true; //logged into the admin system
                }

                return result;
            }
        }

        public static String ToAbsoluteUrl(String path)
        {
            Control resolver = new Control();

            path = resolver.ResolveUrl(path);
            if (!path.StartsWith("/"))
                path = "/" + path;

            return "http://" + CurrentSite.ProductionDomain + path;
        }

        public static Boolean IsProductionHost
        {
            get { return !IsStagingHost; }
        }

        /// <summary>
        /// Gets the templates for the current site's theme.
        /// </summary>
        /// <returns></returns>
        public static IList<CmsTemplate> GetTemplates()
        {
            IList<CmsTemplate> result = CurrentSite.Cache.Get<IList<CmsTemplate>>("currenttemplates");
            if (result == null)
            {
                CmsTheme theme = GetCurrentTheme();
                result = TemplateManager.Instance.GetTemplates(theme);
                CurrentSite.Cache.Add("currenttemplates", result);
            }
            return result;
        }

        public static String GetContainerUrl(String container)
        {
            String fullname = SiteHelper.GetStorageKey(container, CurrentSite.Guid.Value);
            return StorageHelper.GetStorageClient().GetContainerInfo(fullname).Uri.ToString();
        }

        public static String GetImageUrl(String imagename)
        {
            StorageFile file = StorageHelper.GetStorageClient().GetInfo(CurrentSite.ImageStorageDirectory, StorageClientConst.RootFolder, imagename);
            return file.Url;
        }

        public static string Culture 
        {
            get { return "en-us"; } 
        }

        public static string Protocol
        {
            get { return "http://"; }
        }

        public static string GetDefaultTemplateName()
        {
            String name = null;
            CmsTheme theme = ThemeManager.Instance.GetDefaultBySite(CurrentSite.Guid);
            if (theme != null)
            {
                IList<CmsTemplate> templates = TemplateManager.Instance.GetTemplates(theme);
                if (templates.Count > 0)
                {
                    name = templates[0].Name;
                }
            }

            return name;
        }

        public static void RefreshPageCache()
        {
            SitePageCacheRefreshInvoker.InvokeRefresh(Guid.Value, SitePageRefreshRequest.PageRefreshType.All);
        }

        public static bool IsAvailable
        {
            get
            {
                Boolean result;
                try
                {
                    Data.Guid test = CurrentSite.Guid;
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }

                return result;
            }
        }
    }
}
