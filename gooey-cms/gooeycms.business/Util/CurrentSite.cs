﻿using System;
using System.Collections.Generic;
using Gooeycms.Business.Cache;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Data.Model;

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

        public static class Configuration
        {
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

            public static String HeaderImageTemplate
            {
                get { return GetSiteConfiguration("markup-headerimage", "", false); }
            }

            public static Boolean IsLeadEmailEnabled
            {
                get
                {
                    String value = GetSiteConfiguration("lead-email-enabled", "false", false);
                    return Boolean.Parse(value);
                }
            }

            public static Boolean IsLeadDbEnabled
            {
                get
                {
                    String value = GetSiteConfiguration("lead-db-enabled", "true", false);
                    return Boolean.Parse(value);
                }
            }
        }

        private static String GetStorageKey(String type)
        {
            return String.Format(type, SiteHelper.GetActiveSiteGuid(true).Value);
        }

        public static String PageStorageContainer
        {
            get { return GetStorageKey(SiteHelper.PageDirectoryKey); }
        }

        public static String JavascriptStorageContainer
        {
            get { return GetStorageKey(SiteHelper.JavascriptDirectoryKey); }
        }

        public static String StylesheetStorageContainer
        {
            get { return GetStorageKey(SiteHelper.StylesheetDirectoryKey); }
        }

        public static String ImageStorageDirectory
        {
            get { return GetStorageKey(SiteHelper.ImagesDirectoryKey); }
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
                if (Extensions.EqualsCaseInsensitive(StagingDomain, domain))
                    result = true;
                else if (Extensions.EqualsCaseInsensitive(ProductionDomain, domain))
                    result = false;
                else if (LoggedInUser.IsLoggedIn)
                {
                    result = true; //logged into the admin system
                }

                return result;
            }
        }

        public static Boolean IsSalesForceEnabled
        {
            get
            {
                //TODO Implement the code to determine if sales-force is available
                return false;
            }
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
    }
}
