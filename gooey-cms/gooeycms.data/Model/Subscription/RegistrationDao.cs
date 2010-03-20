using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class RegistrationDao : BaseDao
    {
        public Registration FindByGuid(String guid)
        {
            String hql = "select registration from Registration registration where registration.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid).UniqueResult<Registration>();
        }
    }
}
