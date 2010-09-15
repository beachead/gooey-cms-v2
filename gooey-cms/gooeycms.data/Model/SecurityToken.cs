using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model
{
    public class SecurityTokenDao : BaseDao
    {
        public SecurityToken FindByToken(string token)
        {
            String hql = "select token from SecurityToken token where token.Token = :token";
            return base.NewHqlQuery(hql).SetString("token", token).UniqueResult<SecurityToken>();
        }
    }

    public class SecurityToken : BasePersistedItem
    {
        public virtual String Token { get; set; }
        public virtual DateTime Issued { get; set; }
        public virtual DateTime Expires { get; set; }
        public virtual Int32 Uses { get; set; }
        public virtual Int32 MaxUses { get; set; }
    }
}
