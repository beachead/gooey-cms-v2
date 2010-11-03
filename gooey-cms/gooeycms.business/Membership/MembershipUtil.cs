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
                using (Transaction tx = new Transaction())
                {
                    dao.SaveObject(info);
                    tx.Commit();
                }

                //Create the account in the asp.net membership system
                String password = Decrypt(registration.EncryptedPassword);
                try
                {
                    user = System.Web.Security.Membership.CreateUser(registration.Email, password, registration.Email);
                    System.Web.Security.Roles.AddUserToRole(registration.Email, SecurityConstants.DOMAIN_ADMIN);
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

            MembershipUserWrapper wrapper = new MembershipUserWrapper();
            wrapper.MembershipUser = user;
            wrapper.UserInfo = info;

            return wrapper;
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

        public static void ProcessLogin(string username)
        {
            //Immediately expire any existing cookies
            try
            {
                HttpContext.Current.Response.Cookies["selected-site"].Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                HttpContext.Current.Request.Cookies.Remove("selected-site");
            }
            catch (Exception) { }

            MembershipUserWrapper wrapper = FindByUsername(username);
            IList<CmsSubscription> subscriptions = SubscriptionManager.GetSubscriptionsByUserId(wrapper.UserInfo.Id);

            SiteHelper.SetActiveSiteCookie(subscriptions);
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
                dao.SaveObject(info);
                tx.Commit();
            }

            MembershipUser user = System.Web.Security.Membership.CreateUser(DemoAccountUsername, DemoAccountPassword, DemoAccountUsername);
            System.Web.Security.Roles.AddUserToRole(DemoAccountUsername, SecurityConstants.DOMAIN_ADMIN);

            MembershipUserWrapper wrapper = new MembershipUserWrapper();
            wrapper.MembershipUser = user;
            wrapper.UserInfo = info;

            return wrapper;
        }
    }
}
