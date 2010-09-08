using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Forms.Plugins
{
    public class LeadEmailPlugin : FormPlugin
    {
        public override bool IsEnabled()
        {
            return CurrentSite.Configuration.IsLeadEmailEnabled; 
        }

        public override void Process()
        {
        }
    }
}
