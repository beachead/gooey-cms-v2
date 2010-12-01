using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Subscription
{
    public class SubscriptionAdapter
    {
        public SubscriptionAdapter()
        {
        }

        public IList<CmsSubscription> GetUpcomingRenewals(Int32 timeframeInDays)
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindUpcomingRenewals(timeframeInDays);
        }

        public IList<CmsSubscription> GetAllSubscriptions()
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            List<CmsSubscription> results = new List<CmsSubscription>(dao.FindUserSubscriptions());

            results.Sort((x, y) => x.PrimaryUser.Username.CompareTo(y.PrimaryUser.Username));
            return results;
        }
    }
}
