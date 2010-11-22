using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Crypto;
using System.Web.Security;

namespace Gooeycms.Business.Membership
{
    public class MembershipDataSource
    {
        /// <summary>
        /// Deletes the specified user from the system
        /// </summary>
        /// <param name="guid"></param>
        public void DeleteUser(UserInfo userAdapter)
        {
            MembershipUtil.DeleteUser(userAdapter);
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
            userAdapter.Username = userAdapter.Email;
            MembershipUserWrapper wrapper = MembershipUtil.AddUser(userAdapter.Username, userAdapter.Password, userAdapter.Username, userAdapter.Firstname, userAdapter.Lastname);
            SubscriptionManager.AddUserToSubscription(CurrentSite.Guid, wrapper.UserInfo);
        }

        public IList<UserInfo> GetUsers(String encryptedSiteGuid)
        {
            String siteGuid = TextEncryption.Decode(encryptedSiteGuid);
            CmsSubscription subscription = SubscriptionManager.GetSubscription(siteGuid);
            return MembershipUtil.GetUsersBySite(subscription.Id);
        }
    }
}
