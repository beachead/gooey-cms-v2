using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Gooeycms.Business.Storage;
using System.Threading.Tasks;
using Gooeycms.Business.Pages;

namespace Goopeycms.Worker.Control
{
    public class WorkerRole : RoleEntryPoint
    {
        PageRoleWorker worker = new PageRoleWorker();
        public override void Run()
        {
            worker = new PageRoleWorker();
            worker.IsRunning = true;
            while (worker.IsRunning)
            {
                worker.ProcessMessages();
                Thread.Sleep(100);
            }
        }

        public override void OnStop()
        {
            worker.IsRunning = false;
            base.OnStop();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
