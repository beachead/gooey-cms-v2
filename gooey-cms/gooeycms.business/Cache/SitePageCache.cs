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

        private String GetCacheKey(String subkey)
        {
            String stagingOrProduction = (CurrentSite.IsStagingHost) ? SitePageRefreshRequest.PageRefreshType.Staging.ToString() : SitePageRefreshRequest.PageRefreshType.Production.ToString();
            return GetCacheKey(CurrentSite.Guid.Value, stagingOrProduction, subkey);
        }

        private String GetCacheKey(String guid, String type, String url)
        {
            return guid.ToLower() + "-" + type.ToLower() + "-" + url.ToLower();
        }

        public void AddToCache(Web.CmsUrl url, StringBuilder output)
        {
            String key = GetCacheKey(url.ToString());
            cacheManager.Add(key, output, CacheItemPriority.Normal, null, new SlidingTime(TimeSpan.FromMinutes(20)));
        }

        public Boolean GetIfExists(Web.CmsUrl url, StringBuilder output)
        {
            String key = GetCacheKey(url.ToString());
            Boolean result = false;
            if (cacheManager.Contains(key))
            {
                StringBuilder temp = (StringBuilder)cacheManager.GetData(key);
                output.Clear();
                output.Append(temp.ToString());

                result = true;
            }
            return result;
        }

        internal void Flush(string siteGuid, string url, SitePageRefreshRequest.PageRefreshType pageRefreshType)
        {
            String key = GetCacheKey(siteGuid, pageRefreshType.ToString(), url);
            cacheManager.Remove(key);
        }
    }
}
