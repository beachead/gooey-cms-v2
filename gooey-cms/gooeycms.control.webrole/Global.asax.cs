using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.WindowsAzure.ServiceRuntime;
using Gooeycms.Business.Pages;
using System.Threading;

namespace Gooeycms.Webrole.Control
{
    public class Global : System.Web.HttpApplication
    {
        private static System.Threading.Timer pageProcessThread = null;
        private static Semaphore semaphore = new Semaphore(1, 1);

        protected void Application_Start(object sender, EventArgs e)
        {
            if (!RoleEnvironment.IsAvailable)
            {
                if (pageProcessThread == null)
                {
                    pageProcessThread = new System.Threading.Timer(new System.Threading.TimerCallback((object context) =>
                    {
                        bool acquired = semaphore.WaitOne(TimeSpan.FromSeconds(0));
                        if (acquired)
                        {
                            try
                            {
                                PageRoleWorker worker = new PageRoleWorker();
                                worker.ProcessMessages();
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