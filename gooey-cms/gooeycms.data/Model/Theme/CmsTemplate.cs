using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Theme
{
    [Serializable]
    public class CmsTemplate : BasePersistedItem
    {
        public virtual CmsTheme Theme { get; set; }
        public virtual String Name { get; set; }
        public virtual String SubscriptionGuid { get; set; }
        public virtual Boolean IsGlobalTemplateType { get; set; }
        public virtual DateTime LastSaved { get; set; }
        public virtual String Content { get; set; }
    }
}
