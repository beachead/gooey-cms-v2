using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Subscription.Paypal
{
    public class DebugSubscriptionProcessor : PaypalSubscriptionProcessor
    {
        public override bool IsDebug
        {
            get
            {
                return true;
            }
        }

        public override bool ValidateRequest()
        {
            return true;
        }
    }
}
