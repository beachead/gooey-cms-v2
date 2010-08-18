using System;
using System.Collections.Generic;
using System.Web;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Site;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Util
{
    public static class SiteHelper
    {
        private static Dictionary<String, Data.Guid> siteGuidCache = new Dictionary<string, Data.Guid>();

        public const String PageDirectoryKey = "{0}-cmspages";
        public const String JavascriptDirectoryKey = "{0}-javascripts";
        public const String StylesheetDirectoryKey = "{0}-stylesheets";
        public const String ImagesDirectoryKey = "{0}-images";

        public static void SetActiveSiteCookie(IList<CmsSubscription> items)
        {
            if (items.Count == 1)
                SetActiveSiteCookie(items[0].Guid);
            else
                HttpContext.Current.Response.Cookies["selected-site"].Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
        }

        public static void SetActiveSiteCookie(String guid)
        {
            if (String.IsNullOrEmpty(guid))
                throw new ArgumentException("A site guid must be specified when setting the active site.");

            String encrypted = new TextEncryption().Encrypt(guid);
            HttpContext.Current.Response.Cookies["selected-site"].Value = encrypted;
        }

        public static Data.Guid GetActiveSiteGuid()
        {
            return GetActiveSiteGuid(false);
        }

        /// <summary>
        /// Gets the GUID of the active site.
        /// If it's a production site, it'll return the GUID based upon the domain or staging TLD
        /// If it's in the control panel, it'll return the active site being managed.
        /// </summary>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public static Data.Guid GetActiveSiteGuid(Boolean isRequired)
        {
            if (HttpContext.Current == null)
                throw new ApplicationException("The request context was not availabe. To rerieve the active site you must be running from within IIS or Azure");

            Data.Guid result;

            //Check if we're running outside the admin... if so, base the active site upon the domain
            String host = HttpContext.Current.Request.Url.Host;
            if (GooeyConfigManager.AdminSiteHost.EqualsCaseInsensitive(host))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["selected-site"];
                String guid = (cookie != null) ? (cookie.Value) : "";

                if ((isRequired) && (String.IsNullOrEmpty(guid)))
                    throw new ArgumentException("No site has been selected to manage themes for or cookies are disabled.");

                guid = new TextEncryption().Decrypt(guid);
                result = Data.Guid.New(guid);
            }
            else
            {
                String cacheKey = host.ToLower();
                result = siteGuidCache.GetValue<String, Data.Guid>(cacheKey);
                if (result.IsEmpty())
                {
                    CmsSubscription subscription = SubscriptionManager.GetSubscriptionForDomain(host);
                    if (subscription != null)
                    {
                        result = Data.Guid.New(subscription.Guid);
                        siteGuidCache.Add(cacheKey, result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the production domain name for the specified site.
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <returns></returns>
        public static String GetProductionDomain(Data.Guid siteGuid)
        {
            CmsSubscription subscription = SubscriptionManager.GetSubscription(siteGuid);
            if (subscription == null)
                throw new ArgumentException("Could not find a site matching the site id:" + siteGuid);

            String result;
            if (String.IsNullOrEmpty(subscription.Domain))
                result = subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;
            else
                result = subscription.Domain;

            return result;
        }

        public static String GetStagingDomain(Data.Guid siteGuid)
        {
            CmsSubscription subscription = SubscriptionManager.GetSubscription(siteGuid);
            if (subscription == null)
                throw new ArgumentException("Could not find a site matching the site id:" + siteGuid);

            String result;
            if (String.IsNullOrEmpty(subscription.StagingDomain))
                result = GooeyConfigManager.DefaultStagingPrefix + subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;
            else
                result = subscription.StagingDomain;

            return result;
        }

        /// <summary>
        /// Verifies that the site is configured correctly and if not
        /// performs any necessary setup steps.
        /// </summary>
        /// <param name="siteGuid"></param>
        public static void Configure(Data.Guid siteGuid)
        {
            CmsSitePath path = CmsSiteMap.Instance.GetRootPath(siteGuid);
            if (path == null)
                path = CmsSiteMap.Instance.AddRootDirectory(siteGuid);
        }

        public static String GetStorageKey(String type, String guid)
        {
            return String.Format(type, guid);
        }
    }
}
