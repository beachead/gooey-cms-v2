using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Campaign
{
    public class CmsCampaign : BasePersistedItem
    {
        public virtual String Name { get; set; }
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String TrackingCode { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual String PhoneNumber { get; set; }
    }
}
