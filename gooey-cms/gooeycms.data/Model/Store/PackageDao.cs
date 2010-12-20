using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Gooeycms.Data.Model.Store
{
    public class PackageDao : BaseDao
    {
        public enum ApprovalStatus
        {
            Approved,
            NotApproved,
            Any
        }

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

        public IList<Package> FindByPackageType(string packageType, ApprovalStatus status)
        {
            String hql = "select package from Package package where ";

            if (status == ApprovalStatus.Approved)
                hql = hql + "package.IsApproved = 1 ";
            else if (status == ApprovalStatus.NotApproved)
                hql = hql + "package.IsApproved = 0 ";
            else
                hql = hql + "(package.IsApproved = 1 or package.IsApproved = 0) ";

            if (!String.IsNullOrEmpty(packageType))
                hql = hql + "and package.PackageTypeString = :type ";

            hql = hql + "order by package.Created desc";

            IQuery query = base.NewHqlQuery(hql);
            if (!String.IsNullOrEmpty(packageType))
                query.SetString("type", packageType);

            return query.List<Package>();
        }

        public IList<Package> FindByPackageTypeAndPrice(string packageType, ApprovalStatus status, double min, double max)
        {
            String hql = "select package from Package package where ";

            if (status == ApprovalStatus.Approved)
                hql = hql + "package.IsApproved = 1 ";
            else if (status == ApprovalStatus.NotApproved)
                hql = hql + "package.IsApproved = 0 ";
            else
                hql = hql + "(package.IsApproved = 1 or package.IsApproved = 0) ";

            if (!String.IsNullOrEmpty(packageType))
                hql = hql + "and package.PackageTypeString = :type ";

            hql = hql + "and (package.Price between :min and :max) ";

            hql = hql + "order by package.Created desc";

            IQuery query = base.NewHqlQuery(hql);
            if (!String.IsNullOrEmpty(packageType))
                query.SetString("type", packageType);

            query.SetDouble("min", min);
            query.SetDouble("max", max);

            return query.List<Package>();
        }

        public IList<Package> FindByApprovalStatus(bool isApproved)
        {
            String hql = "select package from Package package where package.IsApproved = :approved order by package.Created asc";
            return base.NewHqlQuery(hql).SetBoolean("approved", isApproved).List<Package>();
        }
    }
}
