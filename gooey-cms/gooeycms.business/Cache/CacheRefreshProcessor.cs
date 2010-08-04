using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Cache
{
    public class CacheRefreshProcessor : IQueueMessageProcessor
    {
        public void Process(object item)
        {
            if (item is CacheRefreshRequest)
            {
                CacheRefreshRequest message = (CacheRefreshRequest)item;
                CacheInstance cache = CacheManager.Instance.GetCache(Data.Guid.New(message.SiteGuid));
                if (message.RefreshKey != null)
                {
                    cache.Clear(message.RefreshKey, false);
                }
                else
                {
                    cache.Clear(false);
                }
            }
        }
    }
}
