using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Cache
{
    public class SitePageCache
    {
        private static SitePageCache instance = new SitePageCache();
        private ICacheManager cacheManager;
        private SitePageCache() 
        {
            cacheManager = CacheFactory.GetCacheManager();
        }

        public static SitePageCache Instance 
        {
            get { return SitePageCache.instance; }
        }

        private String GetSiteKey(String guid, String stagingOrProduction)
        {
            return guid + "-" + stagingOrProduction;
        }

        private Dictionary<String, T> GetLocalSiteCache<T>()
        {
            String stagingOrProduction = (CurrentSite.IsStagingHost) ? SitePageRefreshRequest.PageRefreshType.Staging.ToString() : SitePageRefreshRequest.PageRefreshType.Production.ToString();

            String guid = CurrentSite.Guid.Value;
            String siteKey = GetSiteKey(guid, stagingOrProduction);
            if (!cacheManager.Contains(siteKey))
                cacheManager.Add(siteKey, new Dictionary<String, T>(), CacheItemPriority.Normal, null, new SlidingTime(TimeSpan.FromMinutes(20)));

            Dictionary<String, T> sitecache = (Dictionary<String, T>)cacheManager.GetData(siteKey);

            return sitecache;
        }

        public void AddToCache<T>(Web.CmsUrl url, T value)
        {
            Dictionary<String, T> sitecache = GetLocalSiteCache<T>();

            String key = url.ToString();
            sitecache[key] = value;
        }

        public Boolean GetIfExists<T>(Web.CmsUrl url, ref T item)
        {
            String key = url.ToString();
            Dictionary<String, T> sitecache = GetLocalSiteCache<T>();
            Boolean result = false;
            if (sitecache.ContainsKey(key))
            {
                item = sitecache[key];
                result = true;
            }
            return result;
        }

        public void Flush(string siteGuid, SitePageRefreshRequest.PageRefreshType pageRefreshType)
        {
            if (pageRefreshType == SitePageRefreshRequest.PageRefreshType.All)
            {
                String key = GetSiteKey(siteGuid, SitePageRefreshRequest.PageRefreshType.Staging.ToString());
                cacheManager.Remove(key);

                key = GetSiteKey(siteGuid, SitePageRefreshRequest.PageRefreshType.Production.ToString());
                cacheManager.Remove(key);
            }
            else
            {
                String key = GetSiteKey(siteGuid, pageRefreshType.ToString());
                cacheManager.Remove(key);
            }
        }
    }
}
