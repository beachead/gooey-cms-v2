using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Gooeycms.Business.Membership
{
    public static class LoggedInUser
    {
        public static MembershipUserWrapper Wrapper
        {
            get
            {
                return MembershipUtil.FindByUsername(LoggedInUser.Username);
            }
        }

        public static String Username
        {
            get 
            { 
                String username = null;
                MembershipUser user = System.Web.Security.Membership.GetUser();
                if (user != null)
                    username = user.UserName;

                return username;
            }
        }
    }
}
