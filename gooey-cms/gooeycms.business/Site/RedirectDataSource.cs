using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Site;

namespace Gooeycms.Business.Site
{
    public class RedirectItem
    {
        public String SubscriptionId { get; set; }
        public String RedirectFrom { get; set; }
        public String RedirectTo { get; set; }

        public RedirectItem(CmsSitePath path)
        {
            this.RedirectFrom = path.Url;
            this.RedirectTo = path.RedirectTo;
            this.SubscriptionId = path.SubscriptionGuid;
        }
    }

    public class RedirectDataSource
    {
        public IList<RedirectItem> GetRedirects()
        {
            IList<CmsSitePath> paths = CmsSiteMap.Instance.GetRedirects(CurrentSite.Guid);
            
            IList<RedirectItem> results = new List<RedirectItem>();
            foreach (CmsSitePath path in paths)
                results.Add(new RedirectItem(path));

            return results;
        }

        public void InsertRedirect(String redirectFrom, String redirectTo)
        {
            CmsSiteMap.Instance.AddRedirect(CurrentSite.Guid, redirectFrom, redirectTo);
        }

        public void DeleteRedirect(String redirectFrom)
        {
            CmsSitePath path = CmsSiteMap.Instance.GetPath(CurrentSite.Guid, redirectFrom);
            CmsSiteMap.Instance.Remove(path);
        }
    }
}
