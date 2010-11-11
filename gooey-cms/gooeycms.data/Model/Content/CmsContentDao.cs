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

        public CmsContent FindByGuid(Guid siteId, Guid guid)
        {
            String hql = "select content from CmsContent content where content.SubscriptionId = :siteGuid and content.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteId.Value).SetString("guid", guid.Value).UniqueResult<CmsContent>();
        }

        public IList<CmsContent> FindFilesBySite(Guid siteGuid)
        {
            String hql = "select content from CmsContent content where content.SubscriptionId = :guid and content.ContentType.IsFileType = 1";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsContent>();
        }

        public CmsContent FindByFilename(Guid siteGuid, String filename)
        {

            String hql = "select item from CmsContent item join item._Fields fields where item.SubscriptionId = :guid and fields.Name = 'filename' and fields.Value = :filename";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("filename", filename).UniqueResult<CmsContent>();
        }

        public IList<CmsContent> FindUnapprovedContent(Guid siteGuid)
        {
            String hql = "select content from CmsContent content where content.SubscriptionId = :guid and content.IsApproved = 0";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsContent>();
        }

        public void DeleteAllBySite(Guid siteGuid)
        {
            String hql = "select content from CmsContent content where content.SubscriptionId = :guid";
            IList<CmsContent> results = base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsContent>();
            foreach (CmsContent result in results)
                base.Delete<CmsContent>(result);
        }
    }
}
