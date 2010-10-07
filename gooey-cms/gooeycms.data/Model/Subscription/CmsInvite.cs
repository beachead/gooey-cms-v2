using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsInvite : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String Firstname { get; set; }
        public virtual String Lastname { get; set; }
        public virtual String Email { get; set; }
        public virtual String Token { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Issued { get; set; }
        public virtual DateTime Responded { get; set; }
    }
}
