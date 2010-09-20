﻿using System;
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
            UserInfo info = new UserInfo();
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
            MembershipUser user = null;
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

            MembershipUserWrapper wrapper = new MembershipUserWrapper();
            wrapper.MembershipUser = user;
            wrapper.UserInfo = info;

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

        public static void ProcessLogin(string username)
        {
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
