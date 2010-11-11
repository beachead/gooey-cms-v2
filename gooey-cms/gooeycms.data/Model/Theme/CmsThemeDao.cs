using System;
using System.Collections.Generic;

namespace Gooeycms.Data.Model.Theme
{
    public class CmsThemeDao : BaseDao
    {
        public IList<CmsTheme> FindAllThemes(Data.Guid guid)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).List<CmsTheme>();
        }

        public CmsTheme FindBySiteAndName(Data.Guid guid, String name)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :guid and themes.Name = :name";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).SetString("name",name).UniqueResult<CmsTheme>();
        }

        public CmsTheme FindBySiteAndGuid(Data.Guid siteGuid, Data.Guid themeGuid)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :siteGuid and themes.ThemeGuid = :themeGuid";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid.Value).SetString("themeGuid", themeGuid.Value).UniqueResult<CmsTheme>();
        }

        public CmsTheme FindEnabledBySite(Data.Guid siteGuid)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :siteGuid and themes.IsEnabled = 1";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid.Value).UniqueResult<CmsTheme>();
        }

        public void DeleteAllBySite(Guid siteGuid)
        {
            String hql = "select themes from CmsTheme themes where themes.SubscriptionGuid = :guid";
            IList<CmsTheme> results = base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsTheme>();
            foreach (CmsTheme result in results)
                base.Delete<CmsTheme>(result);
        }
    }
}
