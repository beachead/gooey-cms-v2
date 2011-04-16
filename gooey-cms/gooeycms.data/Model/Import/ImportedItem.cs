using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Import
{
    public class ImportedItem : BasePersistedItem
    {
        public virtual String ImportHash { get; set; }
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String Uri { get; set; }
        public virtual String ContentType { get; set; }
        public virtual String ContentEncoding { get; set; }
        public virtual String Title { get; set; }
        public virtual DateTime Inserted { get; set; }
        public virtual DateTime Expires { get; set; }
    }
}
