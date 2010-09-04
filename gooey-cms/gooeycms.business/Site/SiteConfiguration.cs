using System;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model;

namespace Gooeycms.Business.Web
{
    public static class SiteConfiguration
    {
        private static String GetSiteConfiguration(String key, String def, Boolean required)
        {
            Data.Guid guid = CurrentSite.Guid;
            SiteConfigurationDao dao = new SiteConfigurationDao();

            Gooeycms.Data.Model.SiteConfiguration result = dao.FindByKey(guid, key);
            String value = def;
            if (result != null)
                value = result.Value;

            if ((required) && (String.IsNullOrEmpty(value)))
                throw new ArgumentNullException("The configuration value for the site:" + guid + " and key " + key + " has not been set. This value is required for the site to function properly.");

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
    }
}
