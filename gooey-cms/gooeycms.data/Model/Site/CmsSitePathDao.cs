
using System.Collections.Generic;
using System;
namespace Gooeycms.Data.Model.Site
{
    public class CmsSitePathDao : BaseDao
    {
        public CmsSitePath FindBySiteAndHash(Guid siteGuid, Hash hash)
        {
            string hql = "select path from CmsSitePath path where path.SubscriptionGuid = :guid and path.UrlHash = :hash";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("hash", hash.Value).UniqueResult<CmsSitePath>();
        }

        public int FindNextPosition(Guid siteGuid, int depth)
        {
            string hql = "select max(path.Position) from CmsSitePath path where path.SubscriptionGuid = :guid and path.Depth = :depth";
            int pos = base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetInt32("depth", depth).UniqueResult<int>();
            return pos + 1;
        }

        public System.Collections.Generic.IList<CmsSitePath> FindParentPathsBySite(Guid siteGuid)
        {
            string hql = "select paths from CmsSitePath paths where paths.SubscriptionGuid = :guid and paths.IsDirectory = 1 order by paths.Parent, paths.Url";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsSitePath>();
        }

        public System.Collections.Generic.IList<CmsSitePath> FindAllBySiteGuid(Guid siteGuid)
        {
            string hql = "select paths from CmsSitePath paths where paths.SubscriptionGuid = :guid order by depth asc, position asc";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsSitePath>();
        }

        /// <summary>
        /// Finds all of the children of the specified parent
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IList<CmsSitePath> FindChildren(Data.Guid siteGuid, String parent)
        {
            String hql = "select path from CmsSitePath path where path.SubscriptionGuid = :guid and path.Parent = :url order by path.Position asc";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("url", parent).List<CmsSitePath>();
        }

        public void DeleteAllBySite(Guid siteGuid)
        {
            String hql = "select path from CmsSitePath path where path.SubscriptionGuid = :guid";
            IList<CmsSitePath> results = base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsSitePath>();
            foreach (CmsSitePath result in results)
                base.Delete<CmsSitePath>(result);
        }
    }
}
