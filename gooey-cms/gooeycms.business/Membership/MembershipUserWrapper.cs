using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Gooeycms.Data.Model.Subscription;
using System.Web.Security;

namespace Gooeycms.Business.Membership
{
    public class MembershipUserWrapper
    {
        public virtual MembershipUser @MembershipUser { get; set; }
        public virtual UserInfo @UserInfo { get; set; }

        public MembershipUserWrapper() 
        {
        }

        public MembershipUserWrapper(Data.Model.Subscription.UserInfo user, System.Web.Security.MembershipUser membershipUser)
        {
            this.MembershipUser = membershipUser;
            this.UserInfo = user;
        }

        public Boolean IsValid()
        {
            return ((MembershipUser != null) && (UserInfo != null));
        }
    }
}
