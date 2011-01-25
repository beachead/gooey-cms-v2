using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Subscription
{
    public static class Extensions
    {
        public static String GetForwardNumber(this CmsSubscriptionPhoneNumber number)
        {
            String result = number.ForwardNumber;
            if (CurrentSite.IsAvailable)
            {
                if (String.IsNullOrEmpty(result))
                    result = CurrentSite.Configuration.PhoneSettings.DefaultForwardNumber;
            }

            return result;
        }
    }
}
