using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Crypto;
using System.Web.Security;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Membership
{
    public class MembershipDataSource
    {
        public struct MembershipInsertResult
        {
            public Boolean IsSuccess;
            public Boolean IsExistingUser;
            public Boolean HasException;
            public Exception @Exception;
        }

        /// <summary>
        /// Deletes the specified user from the system
        /// </summary>
        /// <param name="guid"></param>
        public void RemoveUserFromSite(UserInfo userinfo)
        {
            String guid = null;

            if (CurrentSite.IsAvailable)
                guid = CurrentSite.Guid.Value;

            if (String.IsNullOrEmpty(guid))
                throw new ArgumentException("The site guid has not been specified and is required to remove a user from a site");

            RemoveUserFromSite(guid, userinfo);
        }

        public void RemoveUserFromSite(String siteGuid, UserInfo userinfo)
        {
            if (userinfo.Id == 0)
                userinfo = MembershipUtil.FindByUsername(userinfo.Username).UserInfo;

            CmsSubscription subscription = SubscriptionManager.GetSubscription(siteGuid);
            SubscriptionManager.RemoveUserFromSubscription(subscription, userinfo);
        }

        public void UpdateUser(UserInfo userinfo)
        {
            //Find the existing user information
            MembershipUserWrapper wrapper = MembershipUtil.FindByUserGuid(userinfo.Guid);
            MembershipUser user = wrapper.MembershipUser;

            //Find the existing account
            wrapper.UserInfo.Firstname = userinfo.Firstname;
            wrapper.UserInfo.Lastname = userinfo.Lastname;

            MembershipUtil.UpdateUserInfo(wrapper.UserInfo);

            //Check if the user is changing their password
            if (!String.IsNullOrEmpty(userinfo.Password))
            {
                MembershipUtil.ChangePassword(user.UserName,userinfo.Password);
            }
        }

        public void InsertUser(UserInfo userAdapter)
        {
            String guid = null;
            
            if (CurrentSite.IsAvailable)
                guid = CurrentSite.Guid.Value;

            if (String.IsNullOrEmpty(guid))
                throw new ArgumentException("The site guid has not been specified and is required to associate a user to a site");

            InsertUser(CurrentSite.Guid.Value, userAdapter);
        }

        public MembershipInsertResult InsertUser(String siteGuid, UserInfo userAdapter)
        {
            MembershipInsertResult result = new MembershipInsertResult();

            try
            {
                userAdapter.Username = userAdapter.Email;
                //Check if this user already exists, if so, just add them to the subscription
                MembershipUserWrapper existing = MembershipUtil.FindByUsername(userAdapter.Username);
                if (!existing.IsValid())
                {
                    result.IsExistingUser = false;
                    existing = MembershipUtil.AddUser(userAdapter.Username, userAdapter.Password, userAdapter.Username, userAdapter.Firstname, userAdapter.Lastname);
                }
                else
                {
                    result.IsExistingUser = true;
                }

                SubscriptionManager.AddUserToSubscription(siteGuid, existing.UserInfo);

                result.HasException = false;
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.HasException = true;
                result.Exception = e;
                result.IsSuccess = false;
            }

            return result;
        }

        public IList<UserInfo> GetUsers(String siteGuid)
        {
            CmsSubscription subscription = SubscriptionManager.GetSubscription(siteGuid);
            return MembershipUtil.GetUsersBySite(subscription.Id);
        }

        public IList<UserInfo> GetUsers()
        {
            return GetUsers(CurrentSite.Guid.Value);
        }
    }
}
