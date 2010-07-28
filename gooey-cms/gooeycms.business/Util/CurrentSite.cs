using System;
using System.Collections.Generic;
using Gooeycms.Business.Cache;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Theme;

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

        private static String GetStorageKey(String type)
        {
            return String.Format(type, SiteHelper.GetActiveSiteGuid(true).Value);
        }

        public static String PageStorageDirectory
        {
            get { return GetStorageKey(SiteHelper.PageDirectoryKey); }
        }

        public static String JavascriptStorageDirectory
        {
            get { return GetStorageKey(SiteHelper.JavascriptDirectoryKey); }
        }

        public static String StylesheetStorageDirectory
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
            StorageFile file = StorageHelper.GetStorageClient().GetInfo(CurrentSite.ImageStorageDirectory, imagename);
            return file.Url;
        }

        public static string Culture 
        {
            get { return "en-us"; } 
        }
    }
}
