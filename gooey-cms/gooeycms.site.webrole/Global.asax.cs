using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Gooeycms.Constants;

namespace Gooeycms.webrole.sites
{
    public class Global : System.Web.HttpApplication
    {
        void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            Initialization.Initialize(context);
        }

        private class Initialization
        {
            private static bool isInitializedAlready = false;
            private static Object key = new Object();

            public static void Initialize(HttpContext context)
            {
                if (isInitializedAlready)
                {
                    return;
                }

                lock (key)
                {
                    if (isInitializedAlready)
                    {
                        return;
                    }

                    isInitializedAlready = true;
                }
            }
        }
    }
}