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
    }
}
