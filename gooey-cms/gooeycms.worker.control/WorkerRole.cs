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
using Gooeycms.Business.Azure;
using Gooeycms.Business;
using Gooeycms.Business.Import;

namespace Goopeycms.Worker.Control
{
    public class WorkerRole : RoleEntryPoint
    {
        private CancellationTokenSource cancel;
        private Task messageTask;
        private Task iisPingTask;
        private Task importSiteTask;

        private static readonly object iisPingTaskKey = new Object();

        public override void Run()
        {
            this.cancel = new CancellationTokenSource();
            Logging.Database.Write("worker-role", "Starting the message processing thread");
            messageTask = Task.Factory.StartNew(() => StartMessageTask(cancel.Token),TaskCreationOptions.LongRunning);

            Logging.Database.Write("worker-role", "Starting the iis site ping thread");
            iisPingTask = Task.Factory.StartNew(() => StartIisPingTask(cancel.Token), TaskCreationOptions.LongRunning);

            Logging.Database.Write("worker-role", "Starting the import site thread");
            importSiteTask = Task.Factory.StartNew(() => StartImportSiteTask(cancel.Token), TaskCreationOptions.LongRunning);

            Task [] tasks = new Task [] { messageTask, iisPingTask, importSiteTask };

            Logging.Database.Write("worker-role", "All threads successfully started");
            Task.WaitAll(tasks);
        }

        private static void StartImportSiteTask(CancellationToken token)
        {
            ImportSiteWorker worker = new ImportSiteWorker();

            Logging.Database.Write("worker-role-queue", "Import site processing task has started and is listening for messages.");
            while (!token.IsCancellationRequested)
            {
                worker.ProcessMessages();
                Thread.Sleep(100);
            }
            Logging.Database.Write("worker-role-queue", "Import site processing task has detected shutdown.");
        }

        private static void StartIisPingTask(CancellationToken token)
        {
            AppPoolPinger pinger = new AppPoolPinger();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    String lasthost = "";
                    try
                    {
                        pinger.PingGooeyCmsSites();

                        lasthost = GooeyConfigManager.StoreSiteHost;
                        pinger.Ping(lasthost);

                        lasthost = "http://" + GooeyConfigManager.AdminSiteHost;
                        pinger.Ping(lasthost);
                    }
                    catch (Exception e)
                    {
                        Logging.Database.Write("worker-role-iis-error", "Unexpected exception: host:" + lasthost + ", error:" + e.Message + ", stack:" + e.StackTrace);
                    }

                    lock (iisPingTaskKey)
                    {
                        Monitor.Wait(iisPingTaskKey, GooeyConfigManager.PingSleepPeriod);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Database.Write("worker-role-iis-error", "There was an unexpected exception while the iis ping task thread was running, error:" + e.Message + ", stack:" + e.StackTrace);
            }

            Logging.Database.Write("worker-role-iis", "Ping task has detected shutdown.");
        }

        private static void StartMessageTask(CancellationToken token)
        {
            PageRoleWorker worker = new PageRoleWorker();

            Logging.Database.Write("worker-role-queue", "Message processing task has started and is listening for messages.");
            while (!token.IsCancellationRequested)
            {
                worker.ProcessMessages();
                Thread.Sleep(100);
            }
            Logging.Database.Write("worker-role-queue", "Message processing task has detected shutdown.");
        }

        public override void OnStop()
        {
            lock (iisPingTaskKey)
            {
                cancel.Cancel();
                Monitor.Pulse(iisPingTaskKey);
            }

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
