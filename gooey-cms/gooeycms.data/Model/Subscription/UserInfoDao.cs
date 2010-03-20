using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class UserInfoDao : BaseDao
    {
        public UserInfo FindByUsername(string username)
        {
            String hql = "select info from UserInfo info where info.Username = :username";
            return base.NewHqlQuery(hql).SetString("username", username).UniqueResult<UserInfo>();
        }
    }
}
