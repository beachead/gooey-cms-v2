using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Extensions;
using Gooeycms.Constants;

namespace Gooeycms.Data.Model.Campaign
{
    public class CmsCampaignElement : BasePersistedItem
    {
        public CmsCampaignElement()
        {
            this.Guid = System.Guid.NewGuid().ToString();
        }

        public const char ElementSeparator = TextConstants.DefaultSeparator;

        public virtual String Guid { get; set; }
        public virtual CmsCampaign Campaign { get; set; }
        public virtual String Name { get; set; }
        public virtual String Placement { get; set; }
        public virtual Int32 Priority { get; set; }
        public virtual String _Pages { get; set; }
        public virtual String Content { get; set; }

        public virtual IList<String> Pages
        {
            get
            {
                return _Pages.SplitAsList(CmsCampaignElement.ElementSeparator);
            }
        }
    }
}
