using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.WindowsAzure.ServiceRuntime;
using Gooeycms.Business.Pages;
using System.Threading;
using Gooeycms.Business.Import;

namespace Gooeycms.Webrole.Control
{
    public class Global : System.Web.HttpApplication
    {
        private static System.Threading.Timer pageProcessThread = null;
        private static System.Threading.Timer siteImportThread = null;
        private static Semaphore pageProcessSemaphore = new Semaphore(1, 1);
        private static Semaphore siteImportSemaphore = new Semaphore(1, 1);

        protected void Application_Start(object sender, EventArgs e)
        {
            if (!RoleEnvironment.IsAvailable)
            {
                StartPageProcessingThread();
                StartSiteImportThread();
            }
        }

        private static void StartPageProcessingThread()
        {
            if (pageProcessThread == null)
            {
                pageProcessThread = new System.Threading.Timer(new System.Threading.TimerCallback((object context) =>
                {
                    bool acquired = pageProcessSemaphore.WaitOne(TimeSpan.FromSeconds(0));
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
                            pageProcessSemaphore.Release();
                        }
                    }
                }), HttpContext.Current, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            }
        }

        private static void StartSiteImportThread()
        {
            if (siteImportThread == null)
            {
                siteImportThread = new System.Threading.Timer(new System.Threading.TimerCallback((object context) =>
                {
                    bool acquired = siteImportSemaphore.WaitOne(TimeSpan.FromSeconds(0));
                    if (acquired)
                    {
                        try
                        {
                            ImportSiteWorker worker = new ImportSiteWorker();
                            worker.ProcessMessages();
                        }
                        catch (Exception) { }
                        finally
                        {
                            siteImportSemaphore.Release();
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