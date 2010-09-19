using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Content
{
    [Serializable]
    public class CmsContentType : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual Boolean IsGlobalType { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Boolean IsFileType { get; set; }
        public virtual Boolean IsEditorVisible { get; set; }

        private String titleFieldName = "title";
        public virtual String TitleFieldName
        {
            get
            {
                return titleFieldName;
            }
            set
            {
                this.titleFieldName = value;
            }
        }
    }
}
