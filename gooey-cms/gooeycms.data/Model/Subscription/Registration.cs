using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    [Serializable]
    public class Registration : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String ExistingAccountGuid { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual String Email { get; set; }
        public virtual String Firstname { get; set; }
        public virtual String Lastname { get; set; }
        public virtual String Company { get; set; }
        public virtual String Address1 { get; set; }
        public virtual String Address2 { get; set; }
        public virtual String City { get; set; }
        public virtual String State { get; set; }
        public virtual String Zipcode { get; set; }
        public virtual String EncryptedPassword { get; set; }
        public virtual String Sitename { get; set; }
        public virtual String Domain { get; set; }
        public virtual String Staging { get; set; }
        public virtual Int32 TemplateId { get; set; }
        public virtual Int32 SubscriptionPlanId { get; set; }
        public virtual Boolean IsComplete { get; set; }
        public virtual Boolean IsSalesforceEnabled { get; set; }
        public virtual Boolean IsGenericOptionEnabled { get; set; }
        public virtual Boolean IsDemoAccount { get; set; }
    }
}
