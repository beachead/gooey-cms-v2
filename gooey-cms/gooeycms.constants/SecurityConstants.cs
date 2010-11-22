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
            public static IList<String> AllSiteRoles = new List<String>() { GLOBAL_ADMINISTRATOR, SITE_ADMINISTRATOR,
                                                                            SITE_STANDARD_USER, SITE_PAGE_EDITOR, 
                                                                            SITE_CONTENT_EDITOR, SITE_CAMPAIGNS, SITE_PROMOTION};

            public const String GLOBAL_ADMINISTRATOR = "global-administrator";
            public const String SITE_ADMINISTRATOR = "site-administrator";
            public const String SITE_STANDARD_USER = "site-user";
            public const String SITE_PAGE_EDITOR = "page-editor";
            public const String SITE_CONTENT_EDITOR = "content-editor";
            public const String SITE_CAMPAIGNS = "campaign-manager";
            public const String SITE_PROMOTION = "promotion-manager";


            public static String GetRoleString(params String[] roles)
            {
                List<String> items = new List<string>(roles);
                return items.AsString<String>(",");
            }
        }        
    }
}
