using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Subscription
{
    public class SubscriptionRestrictionException : Exception
    {
        public enum RestrictionType
        {
            MaxPageCountReached,
            IllegalScriptTags
        }

        public  RestrictionType TypeOfRestriction { get; set; }
        public SubscriptionRestrictionException(string p, RestrictionType type) : base(p)
        {
            this.TypeOfRestriction = type;
        }
    }
}
