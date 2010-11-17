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
        public virtual Int32 MaxAllowedPages { get; set; }
        public virtual Boolean IsJavascriptAllowed { get; set; }
        public virtual Boolean IsExternalImagesAllowed { get; set; }
        public virtual Boolean IsPlanOptionsAllowed { get; set; }
        public virtual Boolean IsLeadsEmailOnly { get; set; }
    }
}
