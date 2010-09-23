using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Content
{
    public class CmsContentDao : BaseDao
    {
        public IList<CmsContent> FindAllContent(Guid siteGuid)
        {
            String hql = "select content from CmsContent content where content.SubscriptionId = :guid";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsContent>();
        }

        public IList<CmsContent> FindContentByType(Guid siteGuid, CmsContentType filter)
        {
            String hql = "select content from CmsContent content where content.SubscriptionId = :guid and content.ContentType.Guid = :typeGuid";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("typeGuid", filter.Guid).List<CmsContent>();
        }

        public CmsContent FindByGuid(Guid guid)
        {
            String hql = "select content from CmsContent content where content.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).UniqueResult<CmsContent>();
        }

        public CmsContent FindByGuid(Guid siteId, Guid guid)
        {
            String hql = "select content from CmsContent content where content.SubscriptionId = :siteGuid and content.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteId.Value).SetString("guid", guid.Value).UniqueResult<CmsContent>();
        }
    }
}
