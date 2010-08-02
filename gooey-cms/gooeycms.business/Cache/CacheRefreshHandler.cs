using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Cache
{
    public class CacheRefreshHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            String token = context.Request.QueryString["token"];
            if (TokenManager.IsValid(CacheInstance.CACHE_REFRESH_KEY, token))
            {
                String key = context.Request.QueryString["key"];
                CacheInstance cache = CurrentSite.Cache;
                if (key != null)
                {
                    cache.Clear(key,false);
                }
                else
                {
                    cache.Clear(false);
                }
            }
        }
    }
}
