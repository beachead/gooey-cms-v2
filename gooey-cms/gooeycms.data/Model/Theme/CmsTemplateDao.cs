using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Theme
{
    public class CmsTemplateDao : BaseDao
    {
        public IList<CmsTemplate> FindByThemeId(int themeId)
        {
            String hql = "select templates from CmsTemplate templates where templates.Theme.Id = :id";
            return base.NewHqlQuery(hql).SetInt32("id", themeId).List<CmsTemplate>();
        }

        public IList<CmsGlobalTemplateType> FindGlobalTemplateTypes()
        {
            String hql = "select types from CmsGlobalTemplateType types order by types.Name asc";
            return base.NewHqlQuery(hql).List<CmsGlobalTemplateType>();
        }
    }
}
