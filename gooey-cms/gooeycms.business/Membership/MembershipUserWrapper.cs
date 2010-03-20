using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Membership
{
    public class MembershipUserWrapper
    {
        public virtual MembershipUser @MembershipUser { get; set; }
        public virtual UserInfo @UserInfo { get; set; }

        public Boolean IsValid()
        {
            return ((MembershipUser != null) && (UserInfo != null));
        }
    }
}
