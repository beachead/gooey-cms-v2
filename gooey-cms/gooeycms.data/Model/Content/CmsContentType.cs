using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Content
{
    [Serializable]
    public class CmsContentType : BasePersistedItem
    {
        private String displayName;

        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual Boolean IsGlobalType { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String TitleFieldName { get; set; }
        public virtual Boolean IsFileType { get; set; }
        public virtual Boolean IsEditorVisible { get; set; }

        public virtual String DisplayName
        {
            set { this.displayName = value; }
            get
            {
                if (!String.IsNullOrEmpty(this.displayName))
                    return this.displayName;
                else
                    return this.Name;
            }
        }
    }
}
