using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

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

        public IList<Package> FindByPackageType(string packageType)
        {
            String hql = "select package from Package package ";
            if (!String.IsNullOrEmpty(packageType))
                hql = hql + "where package.PackageTypeString = :type";
            hql = hql + " order by package.Created desc";

            IQuery query = base.NewHqlQuery(hql);
            if (!String.IsNullOrEmpty(packageType))
                query.SetString("type", packageType);

            return query.List<Package>();
        }
    }
}
