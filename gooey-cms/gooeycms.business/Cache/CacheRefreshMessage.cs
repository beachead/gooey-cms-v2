using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Cache
{
    [Serializable]
    public class CacheRefreshRequest
    {
        public String SiteGuid { get; set; }
        public String RefreshKey { get; set; }
        public Boolean RefreshAll { get; set; }
    }
}
