using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Util
{
    public static class CurrentSite
    {
        private const String PageDirectoryKey = "{0}-cmspages";

        public static String PageStorageKey
        {
            get { return StorageKey(PageDirectoryKey); }
        }

        public static String StorageKey(String type)
        {
            return String.Format(type,CookieHelper.GetActiveSiteGuid(true));
        }
    }
}
