using System;
using System.Web.Security;

namespace Gooeycms.Business.Membership
{
    public static class LoggedInUser
    {
        public static Boolean IsLoggedIn
        {
            get
            {
                return (Username != null);
            }
        }

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

        public static String Email
        {
            get
            {
                String email = null;
                MembershipUser user = System.Web.Security.Membership.GetUser();
                if (user != null)
                    email = user.Email;

                return email;
            }
        }
    }
}
