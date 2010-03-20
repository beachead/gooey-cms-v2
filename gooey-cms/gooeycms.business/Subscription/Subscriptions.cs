using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Membership;
using gooeycms.constants;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business.Subscription
{
    public static class Subscriptions
    {
        public static CmsSubscription CreateFromRegistration(Registration registration)
        {
            MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(registration.Email);
            if (!wrapper.IsValid())
                throw new ApplicationException("The registration has not been properly saved to the database.");

            CmsSubscription subscription = new CmsSubscription();
            subscription.Created = DateTime.Now;
            subscription.Subdomain = registration.Sitename;
            subscription.Domain = registration.Domain;
            subscription.StagingDomain = registration.Staging;
            subscription.SubscriptionTypeId = (int)SubscriptionTypes.Standard;
            subscription.Expires = DateTime.Now.AddYears(100);
            subscription.IsDisabled = false;
            subscription.PrimaryUserGuid = wrapper.UserInfo.Guid;

            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            using (Transaction tx = new Transaction())
            {
                dao.SaveObject(subscription);
                tx.Commit();
            }

            using (Transaction tx = new Transaction())
            {
                dao.AddUserToSubscription(wrapper.UserInfo.Id, subscription.Id);
                tx.Commit();
            }

            return subscription;
        }
    }
}
