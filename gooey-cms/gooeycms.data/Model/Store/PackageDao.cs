using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Store
{
    public class PackageDao : BaseDao
    {
        public IList<Package> FindByUserId(Int32 userId)
        {
            return base.Session.GetNamedQuery("CmsPackageByUserId").SetParameter("userId", userId).List<Package>();
        }

        public Package FindBySiteGuid(Guid siteGuid)
        {
            return null;
        }

        public Package FindByPackageGuid(Guid packageGuid)
        {
            String hql = "select package from Package package where package.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", packageGuid.Value).UniqueResult<Package>();
        }
    }
}
