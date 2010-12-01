using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Billing
{
    public class BillingHistory : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String PaypalProfileId { get; set; }
        public virtual String TxType { get; set; }
        public virtual String TxDescription { get; set; }
        public virtual Double TxAmount { get; set; }
        public virtual String TxId { get; set; }
        public virtual DateTime TxDate { get; set; }
    }
}
