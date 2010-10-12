using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Form
{
    public class CmsSavedForm : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String UserGuid { get; set; }
        public virtual String Name { get; set; }
        public virtual String Markup { get; set; }
        public virtual DateTime DateSaved { get; set; }
    }
}
