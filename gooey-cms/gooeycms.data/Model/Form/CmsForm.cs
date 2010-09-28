using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Form
{
    public class CmsForm : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual DateTime Inserted { get; set; }
        public virtual String FormUrl { get; set; }
        public virtual String IpAddress { get; set; }
        public virtual String Email { get; set; }
        public virtual String RawCampaigns { get; set; }
        public virtual String DownloadedFile { get; set; }
        public virtual String _FormKeys { get; set; }
        public virtual String _FormValues { get; set; }
    }
}
