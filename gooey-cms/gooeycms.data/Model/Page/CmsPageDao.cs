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

        public System.Collections.Generic.IList<CmsPage> SearchByUrl(Data.Guid siteGuid, string filter)
        {
            String hql = "select page from CmsPage page where page.Id in " +
                         "(select max(page.Id) from CmsPage page where page.SubscriptionId = :siteGuid and (" +
                         "(page.Url like '%' + :filter) or (page.Url like :filter + '%') or " +
                         "(page.Url like '%' + :filter + '%')) group by page.Url) " +
                         "order by page.Url asc";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid.Value).SetString("filter", filter).List<CmsPage>();
        }

        public System.Collections.Generic.IList<CmsPage> FindUnapprovedPages(Guid siteGuid, Hash hash)
        {
            String hql = "select page from CmsPage page where page.IsApproved = 0 and page.SubscriptionId = :guid and page.UrlHash = :hash order by page.DateSaved desc";
            return base.NewHqlQuery(hql).SetString("guid",siteGuid.Value).SetString("hash", hash.Value).List<CmsPage>();
        }

        public System.Collections.Generic.IList<CmsPage> FindAllPages(Guid siteGuid, Hash hash)
        {
            String hql = "select page from CmsPage page where page.SubscriptionId = :guid and page.UrlHash = :hash order by page.DateSaved desc";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("hash", hash.Value).List<CmsPage>();
        }

        public System.Collections.Generic.IList<CmsPage> FindApprovedPages(Guid siteGuid, Hash hash)
        {
            String hql = "select page from CmsPage page where page.IsApproved = 1 and page.SubscriptionId = :guid and page.UrlHash = :hash order by page.DateSaved desc";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("hash", hash.Value).List<CmsPage>();
        }

        public System.Collections.Generic.IList<CmsPage> FindByPageHash(Guid siteGuid, Data.Hash hash)
        {
            String hql = "select page from CmsPage page where page.SubscriptionId = :guid and page.UrlHash = :hash";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("hash", hash.Value).List<CmsPage>();
        }
    }
}
