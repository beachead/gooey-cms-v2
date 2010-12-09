using System;
using System.Web.Security;
using Gooeycms.Constants;

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

        public static Boolean IsDemoAccount
        {
            get
            {
                Boolean result = false;
                if (IsLoggedIn)
                    result = (MembershipUtil.DemoAccountUsername.Equals(Username));

                return result;
            }
        }

        public static Boolean IsInRole(params String [] rolenames)
        {
            Boolean result = false;
            if (rolenames.Length > 0)
            {
                foreach (String role in rolenames)
                {
                    result = System.Web.Security.Roles.IsUserInRole(role);
                    if (result)
                        break;
                }
            }
            return result;
        }

        public static bool IsGlobalAdmin
        {
            get { return IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR); }
        }
    }
}
