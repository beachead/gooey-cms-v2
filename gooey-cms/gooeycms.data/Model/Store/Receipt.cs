using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Store
{
    public class Receipt : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String PackageGuid { get; set; }
        public virtual String UserGuid { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Processed { get; set; }
        public virtual Boolean IsComplete { get; set; }
        public virtual String TransactionId { get; set; }
        public virtual Double Amount { get; set; }
        public virtual DateTime PaidDeveloperOn { get; set; }
        public virtual Double PaidDeveloperAmount { get; set; }
    }
}
