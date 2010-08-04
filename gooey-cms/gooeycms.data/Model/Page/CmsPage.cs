using System;

namespace Gooeycms.Data.Model.Page
{
    [Serializable]
    public class CmsPage : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String Culture { get; set; }
        public virtual DateTime DateSaved { get; set; }
        public virtual String Author { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual String ApprovedBy { get; set; }
        public virtual String Title { get; set; }
        public virtual String Description { get; set; }
        public virtual String Keywords { get; set; }
        public virtual String OnBodyLoad { get; set; }
        public virtual String Template { get; set; }
        public virtual String Javascript { get; set; }
        public virtual String Stylesheet { get; set; }
        public virtual String Content { get; set; }
        public virtual String Url { get; set; }
        public virtual String UrlHash { get; set; }
    }
}
