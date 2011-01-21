using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using System.Web.Security;
using Gooeycms.Business.Crypto;
using Beachead.Persistence.Hibernate;
using Gooeycms.Constants;
using Gooeycms.Business.Subscription;
using System.Web;
using Gooeycms.Business.Util;
using Gooeycms.Extensions;

namespace Gooeycms.Business.Membership
{
    /// <summary>
    /// Helper methods to interact with the ASP.NET Membership System.
    /// </summary>
    public static class MembershipUtil
    {
        public const String DemoAccountUsername = "demos@gooeycms.net";
        public const String DemoAccountPassword = "gooeycms135!";

        public static MembershipUserWrapper CreateFromRegistration(Registration registration)
        {
            Boolean exists = (System.Web.Security.Membership.GetUser(registration.Email) != null);

            MembershipUserWrapper wrapper = new MembershipUserWrapper();
            if (String.IsNullOrEmpty(registration.ExistingAccountGuid))
            {
                UserInfo info = new UserInfo();
                MembershipUser user = null;

                info.Username = registration.Email;
                info.Email = registration.Email;
                info.Firstname = registration.Firstname;
                info.Lastname = registration.Lastname;
                info.Company = registration.Company;
                info.Address1 = registration.Address1;
                info.Address2 = registration.Address2;
                info.City = registration.City;
                info.State = registration.State;
                info.Zipcode = registration.Zipcode;
                info.Guid = System.Guid.NewGuid().ToString();
                info.Created = DateTime.Now;

                UserInfoDao dao = new UserInfoDao();
                if (!exists)
                {
                    using (Transaction tx = new Transaction())
                    {
                        dao.SaveObject(info);
                        tx.Commit();
                    }

                }

                //Create the account in the asp.net membership system
                String password = Decrypt(registration.EncryptedPassword);
                try
                {
                    if (!exists)
                        user = System.Web.Security.Membership.CreateUser(registration.Email, password, registration.Email);

                    if (!System.Web.Security.Roles.IsUserInRole(registration.Email, SecurityConstants.Roles.SITE_ADMINISTRATOR))
                        System.Web.Security.Roles.AddUserToRole(registration.Email, SecurityConstants.Roles.SITE_ADMINISTRATOR);
                }
                catch (MembershipCreateUserException e)
                {
                    //Rollback the user info
                    using (Transaction tx = new Transaction())
                    {
                        dao.DeleteObject(info);
                        tx.Commit();
                    }
                    throw e;
                }

                wrapper.MembershipUser = user;
                wrapper.UserInfo = info;
            }
            else
            {
                UserInfo info = new UserInfoDao().FindByGuid(registration.ExistingAccountGuid);
                if (info != null)
                {
                    //make sure the email addresses match
                    if (info.Email.EqualsCaseInsensitive(registration.Email))
                        wrapper = FindByUsername(info.Email);
                }
            }

            return wrapper;
        }

        /// <summary>
        /// Utility method to decrypt text
        /// </summary>
        /// <param name="text"></param>
        public static String Decrypt(String text)
        {
            TextEncryption crypto = new TextEncryption();
            return crypto.Decrypt(text);
        }

        /// <summary>
        /// Finds the membership information based upon the username
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static MembershipUserWrapper FindByUsername(String username)
        {
            MembershipUser user = System.Web.Security.Membership.GetUser(username);
            UserInfo info = new UserInfoDao().FindByUsername(username);

            //Check if the asp.net membership account is orphaned, if so, delete it
            if ((user != null) && (info == null))
            {
                System.Web.Security.Membership.DeleteUser(user.UserName);
                user = null;
            }

            //Check if the gooeycms account is orphaned, if so, delete it
            if ((user == null) && (info != null))
            {
                UserInfoDao dao = new UserInfoDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<UserInfo>(info);
                    tx.Commit();
                }
                info = null;
            }

            MembershipUserWrapper wrapper = new MembershipUserWrapper();
            wrapper.MembershipUser = user;
            wrapper.UserInfo = info;

            return wrapper;
        }

        /// <summary>
        /// Gets all of the users for the specified site
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <returns></returns>
        public static IList<UserInfo> GetUsersBySite(Int32 sitePrimaryKey)
        {
            UserInfoDao dao = new UserInfoDao();
            return dao.FindBySiteGuid(sitePrimaryKey);
        }

        public static MembershipUserWrapper FindByUserGuid(Data.Guid userGuid)
        {
            UserInfoDao dao = new UserInfoDao();
            UserInfo user = dao.FindByGuid(userGuid);
            if (user == null)
                throw new MembershipException("The specified user guid is not valid");

            MembershipUser user2 = System.Web.Security.Membership.GetUser(user.Username);
            return new MembershipUserWrapper(user, user2);
        }

        public static String ProcessLogin(string username)
        {
            String result = null;

            MembershipUserWrapper wrapper = FindByUsername(username);
            IList<CmsSubscription> subscriptions = SubscriptionManager.GetSubscriptionsByUserId(wrapper.UserInfo.Id);

            //Check if there is a current site active
            Boolean isValidSubscription = false;

            if (CurrentSite.IsAvailable)
            {
                String expectedGuid = CurrentSite.Guid.Value;
                result = expectedGuid;

                //Make sure that the site is valid for this subscription
                isValidSubscription = subscriptions.Any(s => s.Guid.Equals(expectedGuid));
            }

            //Find a valid subscription
            if (!isValidSubscription)
            {
                if (subscriptions.Count > 0)
                {
                    CmsSubscription defaultSubscription = subscriptions[0];
                    SiteHelper.SetActiveSiteCookie(defaultSubscription.Guid);

                    result = defaultSubscription.Guid;
                }
            }

            return result;
        }

        public static MembershipUserWrapper CreateDemoAccount()
        {
            UserInfo info = new UserInfo();
            info.Username = DemoAccountUsername;
            info.Email = DemoAccountUsername;
            info.Firstname = "GooeyCMS Demo";
            info.Lastname = "Account";
            info.Company = "GooeyCMS";
            info.Address1 = "135 Gooey Ave";
            info.Address2 = null;
            info.City = "New York";
            info.State = "NY";
            info.Zipcode = "10114";
            info.Guid = System.Guid.NewGuid().ToString();
            info.Created = DateTime.Now;

            UserInfoDao dao = new UserInfoDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<UserInfo>(info);
                tx.Commit();
            }

            MembershipUser user = System.Web.Security.Membership.CreateUser(DemoAccountUsername, DemoAccountPassword, DemoAccountUsername);
            System.Web.Security.Roles.AddUserToRole(DemoAccountUsername, SecurityConstants.Roles.SITE_ADMINISTRATOR);

            MembershipUserWrapper wrapper = new MembershipUserWrapper();
            wrapper.MembershipUser = user;
            wrapper.UserInfo = info;

            return wrapper;
        }

        public static Boolean IsValidUsername(String username)
        {
            return true;
        }

        public static Boolean IsValidEmail(String email)
        {
            return true;
        }

        public static void UpdateUserInfo(UserInfo userinfo)
        {
            UserInfoDao dao = new UserInfoDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<UserInfo>(userinfo);
                tx.Commit();
            }
        }

        public static Boolean IsUserInRole(String username, String rolename)
        {
            return System.Web.Security.Roles.IsUserInRole(username, rolename);
        }

        public static MembershipUserWrapper AddUser(String username, String password, String email, String firstname, String lastname)
        {
            MembershipUserWrapper existing = FindByUsername(username);
            if (existing.MembershipUser != null)
                throw new MembershipException("The username " + username + " already exists and may not be used again.");

            if (!IsValidUsername(username))
                throw new MembershipException("The username " + username + " is not valid and may not be used.");

            if (!IsValidEmail(email))
                throw new MembershipException("The email " + email + " is not valid and may not be used.");

            UserInfo info = new UserInfo();
            info.Guid = Guid.NewGuid().ToString();
            info.Firstname = firstname;
            info.Lastname = lastname;
            info.Username = username;
            info.Email = email;
            info.Created = DateTime.Now;

            UserInfoDao dao = new UserInfoDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<UserInfo>(info);
                tx.Commit();
            }

            MembershipUser user = System.Web.Security.Membership.CreateUser(info.Username, password, info.Email);
            return new MembershipUserWrapper(info, user);
        }

        public static void AddUserToRole(string username, string rolename)
        {
            if (!Roles.RoleExists(rolename))
                Roles.CreateRole(rolename);

            if (!Roles.IsUserInRole(username, rolename))
                System.Web.Security.Roles.AddUserToRole(username, rolename);
        }

        public static void RemoveUserFromRole(string username, string rolename)
        {
            if (Roles.IsUserInRole(username,rolename))
                Roles.RemoveUserFromRole(username, rolename);
        }

        internal static void DeleteUser(UserInfo userAdapter)
        {
            UserInfoDao dao = new UserInfoDao();
            UserInfo user = dao.FindByGuid(userAdapter.Guid);
            if (user != null)
            {
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<UserInfo>(user);
                    tx.Commit();
                }
                System.Web.Security.Membership.DeleteUser(user.Username);
            }
        }

        internal static void ChangePassword(String username, String password)
        {
            MembershipUser user = System.Web.Security.Membership.GetUser(username);
            if (user != null)
            {
                user.UnlockUser();
                String temp = user.ResetPassword();
                bool result = user.ChangePassword(temp, password);
            }
        }

        public static bool IsRolesConfigured 
        {
            get
            {
                Boolean result = true;
                //Check if all the roles exist
                foreach (String role in SecurityConstants.Roles.AllSiteRoles)
                {
                    if (!Roles.RoleExists(role))
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        public static void ConfigureRoles()
        {
            foreach (String role in SecurityConstants.Roles.AllSiteRoles)
            {
                if (!Roles.RoleExists(role))
                    Roles.CreateRole(role);
            }
        }

        public static void DeleteAllAccounts(CmsSubscription subscription)
        {
            if (subscription != null)
            {
                IList<UserInfo> users = GetUsersBySite(subscription.Id);
                foreach (UserInfo user in users)
                {
                    DeleteUser(user);
                }
            }
        }
    }
}
