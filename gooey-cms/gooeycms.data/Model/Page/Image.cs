using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Page
{
    public class Image : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String CloudUrl { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual String Filename { get; set; }
        public virtual String ContentType { get; set; }
    }
}
