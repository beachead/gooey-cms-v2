﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using System.Web;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Subscription;

namespace Gooeycms.Business.Util
{
    public static class SiteHelper
    {
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

        public static String GetActiveSiteGuid()
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
        public static String GetActiveSiteGuid(Boolean isRequired)
        {
            if (HttpContext.Current == null)
                throw new ApplicationException("The request context was not availabe. To rerieve the active site you must be running from within IIS or Azure");

            HttpCookie cookie = HttpContext.Current.Request.Cookies["selected-site"];
            String guid = (cookie != null) ? (cookie.Value) : "";

            if ((isRequired) && (String.IsNullOrEmpty(guid)))
                throw new ArgumentException("No site has been selected to manage themes for or cookies are disabled.");

            guid = new TextEncryption().Decrypt(guid);
            return guid;
        }

        /// <summary>
        /// Gets the production domain name for the specified site.
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <returns></returns>
        public static String GetProductionDomain(String siteGuid)
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

        public static String GetStagingDomain(String siteGuid)
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
    }
}
