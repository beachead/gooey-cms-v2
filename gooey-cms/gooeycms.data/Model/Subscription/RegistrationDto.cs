using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class RegistrationDto : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual String Email { get; set; }
        public virtual bool IsComplete { get; set; }
        public virtual byte [] Data { get; set; }
    }
}
