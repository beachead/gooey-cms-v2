using System;
using System.Collections.Generic;
using Gooeycms.Business.Util;
using Gooeycms.Business.Web;
using Gooeycms.Business.Crypto;
using System.Net;
using Microsoft.Security.Application;
using System.Threading;
using System.Threading.Tasks;
using Gooeycms.Business.Azure;

namespace Gooeycms.Business.Cache
{
    public class CacheInstance
    {
        public const String CACHE_REFRESH_KEY = "gooeycmscache";

        private Dictionary<String, Object> table = new Dictionary<string, object>();

        public CacheInstance()
        {
        }

        public ResultType Get<ResultType>(String key)
        {
            return (ResultType)table.GetValue(key);
        }

        public void Add(String key, Object item)
        {
            table[key] = item;
        }

        internal void Clear(Boolean refresh)
        {
            table.Clear();
            if (refresh)
                RefreshStaging(null);
        }

        internal void Clear(string key, Boolean refresh)
        {
            table[key] = null;
            if (refresh)
                RefreshStaging(key);
        }

        public void Clear()
        {
            Clear(true);
        }


        internal void Clear(string key)
        {
            Clear(key,true);
        }

        private void RefreshStaging(String key)
        {
            CacheRefreshRequest message = new CacheRefreshRequest();
            message.SiteGuid = CurrentSite.Guid.Value;
            message.RefreshKey = key;
            message.RefreshAll = (key == null);

            Communication.Broadcast<CacheRefreshRequest>(typeof(CacheRefreshProcessor), message);
            /*
            String url = CurrentSite.Protocol + CurrentSite.StagingDomain +"/cacherefresh.handler?token=" + AntiXss.UrlEncode(TokenManager.Issue(CACHE_REFRESH_KEY, TimeSpan.FromSeconds(30)));
            if (key != null)
                url = url + "&key=" + AntiXss.UrlEncode(key);

            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.GetResponse();
                    }
                    catch(Exception) {}
                });
             */
        }
   } 
}
