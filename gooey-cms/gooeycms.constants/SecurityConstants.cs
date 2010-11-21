using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Extensions;

namespace Gooeycms.Constants
{
    public static class SecurityConstants
    {
        public static class Roles
        {
            public const String GLOBAL_ADMINISTRATOR = "global-administrator";
            public const String SITE_ADMINISTRATOR = "site-administrator";
            public const String SITE_STANDARD_USER = "site-user";
            public const String SITE_PAGE_EDITOR = "site-page-editor";
            public const String SITE_CONTENT_EDITOR = "site-content-editor";
            public const String SITE_CAMPAIGNS = "site-campaigns";

            public static String GetRoleString(params String[] roles)
            {
                List<String> items = new List<string>(roles);
                return items.AsString<String>(",");
            }
        }        
    }
}
