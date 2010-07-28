using System.Collections.Generic;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Cache
{
    public class CacheManager
    {
        private static CacheManager instance = new CacheManager();
        private static IDictionary<Data.Guid, CacheInstance> caches = new Dictionary<Data.Guid, CacheInstance>();

        private CacheManager() { }
        public static CacheManager Instance
        {
            get { return CacheManager.instance; }
        }

        public void ClearAll()
        {
            lock (caches)
            {
                caches.Clear();
            }
        }
        public CacheInstance GetCache(Data.Guid guid)
        {
            CacheInstance table = caches.GetValue(guid);
            if (table == null)
            {
                lock (caches)
                {
                    if (table == null)
                    {
                        table = new CacheInstance();
                        caches.Add(guid, table);
                    }
                }
            }

            return table;
        }
    }
}

