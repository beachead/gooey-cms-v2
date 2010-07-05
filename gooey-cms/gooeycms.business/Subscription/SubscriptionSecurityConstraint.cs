using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Subscription
{
    public class SubscriptionSecurityConstraint : Exception
    {
        public SubscriptionSecurityConstraint(String msg) : base(msg) { }
    }
}
