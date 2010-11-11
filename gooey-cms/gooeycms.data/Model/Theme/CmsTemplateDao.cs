using System;
using System.Collections.Generic;

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

        public CmsTemplate FindBySiteAndName(Guid siteGuid, string templateName)
        {
            String hql = "select template from CmsTemplate template where template.SubscriptionGuid = :guid and template.Name = :name";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("name", templateName).UniqueResult<CmsTemplate>();
        }

        public void DeleteAllBySite(Guid siteGuid)
        {
            String hql = "select templates from CmsTemplate templates where templates.SubscriptionGuid = :guid";
            IList<CmsTemplate> results = base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsTemplate>();
            foreach (CmsTemplate result in results)
                base.Delete<CmsTemplate>(result);
        }
    }
}
