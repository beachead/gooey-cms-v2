using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Page
{
    public class CmsImage : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String CloudUrl { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual String Directory { get; set; }
        public virtual String Filename { get; set; }
        public virtual long Length { get; set; }
        public virtual String ContentType { get; set; }

        public virtual byte[] Data { get; set; }
    }
}
