using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using System.Web;

namespace Gooeycms.Business.Util
{
    public static class CookieHelper
    {
        public static void SetActiveSite(IList<CmsSubscription> items)
        {
            if (items.Count == 1)
                SetActiveSite(items[0].Guid);
            else
                HttpContext.Current.Response.Cookies["selected-site"].Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
        }

        public static void SetActiveSite(String guid)
        {
            //If there's only one site, set that site cookie now
            HttpContext.Current.Response.Cookies["selected-site"].Value = guid;
        }
    }
}
