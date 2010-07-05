using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscriptionPlan : BasePersistedItem
    {
        public virtual String SKU { get; set; }
        public virtual String Name { get; set; }
        public virtual Decimal Price { get; set; }
        public virtual Decimal SalePrice { get; set; }
        public virtual DateTime SaleExpires { get; set; }
        public virtual Boolean IsAvailable { get; set; }
    }
}
