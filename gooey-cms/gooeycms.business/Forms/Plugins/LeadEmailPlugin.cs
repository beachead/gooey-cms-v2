using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Forms.Plugins
{
    public class LeadEmailPlugin : FormPlugin
    {
        public override bool IsEnabled()
        {
            return SiteConfiguration.IsLeadEmailEnabled; 
        }

        public override void Process()
        {
        }
    }
}
