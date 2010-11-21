using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class UserInfo : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual String Username { get; set; }
        public virtual String Password { get; set; }
        public virtual String Email { get; set; }
        public virtual String Firstname { get; set; }
        public virtual String Lastname { get; set; }
        public virtual String Company { get; set; }
        public virtual String Address1 { get; set; }
        public virtual String Address2 { get; set; }
        public virtual String City { get; set; }
        public virtual String State { get; set; }
        public virtual String Zipcode { get; set; }
    }
}
