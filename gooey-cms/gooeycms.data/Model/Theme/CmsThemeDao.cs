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
    }
}
