﻿using System.Collections.Generic;
using Gooeycms.Business.Util;
using Gooeycms.Extensions;

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
                foreach (CacheInstance cache in caches.Values)
                {
                    cache.Clear();
                }
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
                        table = new CacheInstance(guid);
                        caches.Add(guid, table);
                    }
                }
            }

            return table;
        }
    }
}

