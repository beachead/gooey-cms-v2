using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Store
{
    public class UserPackageDao : BaseDao
    {
        public UserPackage FindByUserAndPackage(Guid userGuid, string packageGuid)
        {
            String hql = "select up from UserPackage up where up.UserGuid = :user and up.PackageGuid = :package";
            return base.NewHqlQuery(hql).SetString("user", userGuid.Value).SetString("package", packageGuid).UniqueResult<UserPackage>();
        }
    }
}
