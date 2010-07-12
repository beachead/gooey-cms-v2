using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Themes;

namespace Gooeycms.Business.Util
{
    public static class CurrentSite
    {
        private const String PageDirectoryKey = "{0}-cmspages";

        public static String GetStorageKey(String type)
        {
            return String.Format(type, SiteHelper.GetActiveSiteGuid(true));
        }

        public static String PageStorageKey
        {
            get { return GetStorageKey(PageDirectoryKey); }
        }

        public static String Guid
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
                //TODO return actual data
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
            CmsTheme theme = GetCurrentTheme();
            return TemplateManager.Instance.GetTemplates(theme);
        }
    }
}
