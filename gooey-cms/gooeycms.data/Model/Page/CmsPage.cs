using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Page
{
    public class CmsPage : BasePersistedItem
    {
        public virtual String Guid { get; set; }
        public virtual String Culture { get; set; }
        public virtual DateTime DateSaved { get; set; }
        public virtual String Author { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual String ApprovedBy { get; set; }
        public virtual String Title { get; set; }
        public virtual String Description { get; set; }
        public virtual String Keywords { get; set; }
        public virtual String Path { get; set; }
        public virtual String Css { get; set; }
        public virtual String Javascript { get; set; }
        public virtual String OnBodyLoad { get; set; }
        public virtual String Markup { get; set; }
        public virtual String Template { get; set; }
    }
}
