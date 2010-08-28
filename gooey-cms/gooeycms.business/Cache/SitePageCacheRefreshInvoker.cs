using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Azure;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Cache
{
    public class SitePageCacheRefreshInvoker
    {
        public static void InvokeRefresh(String siteGuid, SitePageRefreshRequest.PageRefreshType refreshType)
        {
            SitePageRefreshRequest message = new SitePageRefreshRequest();
            message.SiteGuid = siteGuid;
            message.RefreshType = refreshType;

            InstanceCommunication.Broadcast<SitePageRefreshRequest>(typeof(SitePageCacheRefreshProcessor), message);
        }
    }
}
