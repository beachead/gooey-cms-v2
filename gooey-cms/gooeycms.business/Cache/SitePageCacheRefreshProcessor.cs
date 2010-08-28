using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Cache
{
    public class SitePageCacheRefreshProcessor : IQueueMessageProcessor
    {
        public void Process(object item)
        {
            if (item is SitePageRefreshRequest)
            {
                SitePageRefreshRequest request = (SitePageRefreshRequest)item;
                SitePageCache.Instance.Flush(request.SiteGuid,request.RefreshType);
            }
        }
    }
}
