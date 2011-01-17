using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Constants;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscription : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionPlanSku { get { return this.SubscriptionPlan.SKU; } }
        public virtual String PrimaryUserGuid { get { return this.PrimaryUser.Guid; } }
        public virtual String Culture { get; set; }
        public virtual String Subdomain { get; set; }
        public virtual String Domain { get; set; }
        public virtual String StagingDomain { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Expires { get; set; }
        public virtual String PaypalProfileId { get; set; }
        public virtual Boolean IsDisabled { get; set; }
        public virtual Boolean IsSalesforceEnabled { get; set; }
        public virtual Boolean IsCampaignEnabled { get; set; }
        public virtual Boolean IsGenericOptionsEnabled { get; set; }
        public virtual Boolean IsDemo { get; set; }
        public virtual UserInfo PrimaryUser { get; set; }
        public virtual CmsSubscriptionPlan SubscriptionPlan { get; set; }
        public virtual Boolean IsDirty { get; set; }
        public virtual Int32 MaxPhoneNumbers { get; set; }

        public virtual SubscriptionPlans SubscriptionPlanEnum
        {
            get 
            {
                return (SubscriptionPlans)Enum.Parse(typeof(SubscriptionPlans), SubscriptionPlanSku, true);
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
