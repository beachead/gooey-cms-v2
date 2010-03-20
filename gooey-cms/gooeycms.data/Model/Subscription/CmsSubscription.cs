using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gooeycms.constants;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscription : BasePersistedItem
    {
        public virtual Int32 SubscriptionTypeId { get; set; }
        public virtual String PrimaryUserGuid { get; set; }
        public virtual String Subdomain { get; set; }
        public virtual String Domain { get; set; }
        public virtual String StagingDomain { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Expires { get; set; }
        public virtual Boolean IsDisabled { get; set; }

        public virtual SubscriptionTypes SubscriptionType
        {
            get 
            {
                return (SubscriptionTypes)Enum.Parse(typeof(SubscriptionTypes), SubscriptionTypeId.ToString());
            }
        }
    }
}
