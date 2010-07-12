using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Theme
{
    public class CmsThemeDao : BaseDao
    {
        public IList<CmsTheme> FindAllThemes(String guid)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid).List<CmsTheme>();
        }

        public CmsTheme FindBySiteAndName(String guid, String name)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :guid and themes.Name = :name";
            return base.NewHqlQuery(hql).SetString("guid", guid).SetString("name",name).UniqueResult<CmsTheme>();
        }

        public CmsTheme FindBySiteAndGuid(string siteGuid, string themeGuid)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :siteGuid and themes.ThemeGuid = :themeGuid";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid).SetString("themeGuid", themeGuid).UniqueResult<CmsTheme>();
        }

        public CmsTheme FindEnabledBySite(string siteGuid)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :siteGuid and themes.IsEnabled = 1";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid).UniqueResult<CmsTheme>();
        }
    }
}
