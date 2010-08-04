using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Gooeycms.Business.Cache;
using Gooeycms.Business.Azure;
using System.Threading;

namespace Gooeycms.webrole.sites
{
    public class Global : System.Web.HttpApplication
    {
        private static System.Threading.Timer intraProcessCommunicationTimer = null;
        private static Semaphore semaphore = new Semaphore(1, 1);

        protected void Application_Start(object sender, EventArgs e)
        {
            if (intraProcessCommunicationTimer == null)
            {
                intraProcessCommunicationTimer = new System.Threading.Timer(new System.Threading.TimerCallback((object context) =>
                    {
                        bool acquired = semaphore.WaitOne(TimeSpan.FromSeconds(0));
                        if (acquired)
                        {
                            try
                            {
                                IList<WebroleMessage> messages = Communication.ReceiveAllUnread();
                                if (messages.Count > 0)
                                {
                                    Communication.ProcessMessages(messages);
                                }
                            }
                            catch (Exception) { }
                            finally
                            {
                                semaphore.Release();
                            }
                        }
                    }), HttpContext.Current, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}