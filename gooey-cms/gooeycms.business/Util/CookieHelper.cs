using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using System.Web;
using Gooeycms.Business.Crypto;

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
            if (String.IsNullOrEmpty(guid))
                throw new ArgumentException("A site guid must be specified when setting the active site.");

            String encrypted = new TextEncryption().Encrypt(guid);
            HttpContext.Current.Response.Cookies["selected-site"].Value = encrypted;
        }

        public static String GetActiveSiteGuid()
        {
            return GetActiveSiteGuid(false);
        }

        public static String GetActiveSiteGuid(Boolean isRequired)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["selected-site"];
            String guid = (cookie != null) ? (cookie.Value) : "";

            if ((isRequired) && (String.IsNullOrEmpty(guid)))
                throw new ArgumentException("No site has been selected to manage themes for or cookies are disabled.");

            guid = new TextEncryption().Decrypt(guid);
            return guid;
        }
    }
}
