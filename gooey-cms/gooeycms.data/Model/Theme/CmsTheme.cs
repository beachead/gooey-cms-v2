using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Theme
{
    [Serializable]
    public class CmsTheme : BasePersistedItem
    {
        public virtual String SubscriptionGuid { get; set; }
        public virtual String ThemeGuid { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public virtual Boolean IsDeveloper { get; set; }
        public virtual String Header { get; set; }
        public virtual String Footer { get; set; }
    }
}
