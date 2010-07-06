using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Constants;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscription : BasePersistedItem
    {
        public virtual Int32 SubscriptionTypeId { get; set; }
        public virtual Int32 SubscriptionPlanId { get; set; }
        public virtual String Guid { get; set; }
        public virtual String PrimaryUserGuid { get; set; }
        public virtual String Subdomain { get; set; }
        public virtual String Domain { get; set; }
        public virtual String StagingDomain { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Expires { get; set; }
        public virtual Boolean IsDisabled { get; set; }
        public virtual Boolean IsSalesforceEnabled { get; set; }
        public virtual Boolean IsGenericOptionsEnabled { get; set; }

        public virtual SubscriptionPlans SubscriptionType
        {
            get 
            {
                return (SubscriptionPlans)Enum.Parse(typeof(SubscriptionPlans), SubscriptionTypeId.ToString());
            }
        }

        public virtual String DefaultDisplayName
        {
            get
            {
                return (String.IsNullOrEmpty(Domain)) ? Subdomain : Domain;
            }
        }
    }
}
