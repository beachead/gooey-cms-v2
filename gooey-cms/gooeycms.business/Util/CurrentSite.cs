using System;
using System.Collections.Generic;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Theme;

namespace Gooeycms.Business.Util
{
    public static class CurrentSite
    {
        private const String PageDirectoryKey = "{0}-cmspages";
        private const String JavascriptDirectoryKey = "{0}-javascripts";
        private const String StylesheetDirectoryKey = "{0}-stylesheets";

        private static String GetStorageKey(String type)
        {
            return String.Format(type, SiteHelper.GetActiveSiteGuid(true).Value);
        }

        public static String PageStorageDirectory
        {
            get { return GetStorageKey(PageDirectoryKey); }
        }

        public static String JavascriptStorageDirectory
        {
            get { return GetStorageKey(JavascriptDirectoryKey); }
        }

        public static String StylesheetStorageDirectory
        {
            get { return GetStorageKey(StylesheetDirectoryKey); }
        }

        public static Data.Guid Guid
        {
            get { return SiteHelper.GetActiveSiteGuid(true); }
        }

        public static String ProductionDomain
        {
            get { return SiteHelper.GetProductionDomain(CurrentSite.Guid); }
        }

        public static String StagingDomain
        {
            get { return SiteHelper.GetStagingDomain(CurrentSite.Guid); }
        }

        public static CmsTheme GetCurrentTheme()
        {
            return ThemeManager.Instance.GetDefaultBySite(Guid);
        }

        public static Boolean IsStagingHost
        {
            get
            {
                WebRequestContext context = new WebRequestContext();
                String domain = context.Request.Url.Host;

                Boolean result = false;
                //check if we're on the staging site
                if (StringExtensions.EqualsCaseInsensitive(StagingDomain, domain))
                    result = true;
                else if (StringExtensions.EqualsCaseInsensitive(ProductionDomain, domain))
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
            CmsTheme theme = GetCurrentTheme();
            return TemplateManager.Instance.GetTemplates(theme);
        }

        public static string Culture 
        {
            get { return "en-us"; } 
        }
    }
}
