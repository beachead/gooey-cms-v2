﻿using System;
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

        public UserInfo FindByGuid(Data.Guid guid)
        {
            String hql = "select info from UserInfo info where info.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).UniqueResult<UserInfo>();
        }

        public IList<UserInfo> FindBySiteGuid(Int32 primaryKey)
        {
            return base.Session.GetNamedQuery("UserInfoBySite").SetParameter("id", primaryKey).List<UserInfo>();            
        }
    }
}
