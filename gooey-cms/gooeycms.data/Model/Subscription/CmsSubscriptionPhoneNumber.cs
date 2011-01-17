using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscriptionPhoneNumber : BasePersistedItem
    {
        public virtual String SubscriptionId { get; set; }
        public virtual String Sid { get; set; }
        public virtual String AccountSid { get; set; }
        public virtual String TwilioInfoUrl { get; set; }
        public virtual String FriendlyPhoneNumber { get; set; }
        public virtual String PhoneNumber { get; set; }
        public virtual String ForwardNumber { get; set; }
    }
}
