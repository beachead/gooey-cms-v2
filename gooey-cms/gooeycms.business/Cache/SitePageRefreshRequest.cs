using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Cache
{
    [Serializable]
    public class SitePageRefreshRequest
    {
        public enum PageRefreshType
        {
            Staging,
            Production,
            All
        }

        public String SiteGuid { get; set; }
        public PageRefreshType RefreshType { get; set; }
    }
}
