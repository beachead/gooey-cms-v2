using System;
using System.Collections.Generic;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscriptionDao : BaseDao
    {
        /// <summary>
        /// Adds a user to the subscription.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="subscriptionId"></param>
        public void AddUserToSubscription(Int32 userId, Int32 subscriptionId)
        {
            base.Session.CreateSQLQuery("insert into User_Subscriptions (user_id,subscription_id) values (:userId, :subscriptionId)")
                .SetParameter("userId", userId)
                .SetParameter("subscriptionId", subscriptionId)
                .ExecuteUpdate();
        }

        public CmsSubscription FindBySubdomain(string subdomain)
        {
            String hql = "select subscription from CmsSubscription subscription where subscription.Subdomain = :domain";
            CmsSubscription result = base.NewHqlQuery(hql).SetString("domain", subdomain).UniqueResult<CmsSubscription>();

            return result;
        }

        public IList<CmsSubscription> FindByUserId(int userId)
        {
            return base.Session.GetNamedQuery("CmsSubscriptionByUserId").SetParameter("userId", userId).List<CmsSubscription>();
        }

        public CmsSubscription FindByGuid(Guid guid)
        {
            String hql = "select subscription from CmsSubscription subscription where subscription.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).UniqueResult<CmsSubscription>();
        }

        public CmsSubscription FindByDomains(string subdomain, string host)
        {
            String hql = "select subscription from CmsSubscription subscription where subscription.Subdomain = :subdomain or subscription.Domain = :domain or subscription.StagingDomain = :staging";
            return base.NewHqlQuery(hql).SetString("subdomain", subdomain).SetString("domain", host).SetString("staging", host).UniqueResult<CmsSubscription>();
        }
    }
}
