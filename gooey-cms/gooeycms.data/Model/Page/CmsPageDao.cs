using System;

namespace Gooeycms.Data.Model.Page
{
    public class CmsPageDao : BaseDao
    {
        /// <summary>
        /// Finds the lates page based upon the site guid, page path, and whether it's approved.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="path"></param>
        /// <param name="approvedOnly"></param>
        public CmsPage FindLatesBySiteAndHash(Data.Guid guid, Data.Hash hash, bool approvedOnly)
        {
            String hql = "select page from CmsPage page where page.SubscriptionId = :guid and page.UrlHash = :hash order by page.Id desc";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).SetString("hash", hash.Value).SetMaxResults(1).UniqueResult<CmsPage>();
        }

        public CmsPage FindByPageGuid(Data.Guid pageGuid)
        {
            String hql = "select page from CmsPage page where page.Guid = :pageGuid";
            return base.NewHqlQuery(hql).SetString("pageGuid", pageGuid.Value).UniqueResult<CmsPage>();
        }
    }
}
