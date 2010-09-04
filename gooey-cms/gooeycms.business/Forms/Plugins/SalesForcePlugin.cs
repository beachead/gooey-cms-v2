using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Forms.Plugins
{
    public class SalesForcePlugin : FormPlugin
    {
        public override bool IsEnabled()
        {
            return CurrentSite.IsSalesForceEnabled;
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
