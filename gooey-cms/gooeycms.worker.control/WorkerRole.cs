﻿using System;
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

namespace Goopeycms.Worker.Control
{
    public class WorkerRole : RoleEntryPoint
    {
        private CancellationTokenSource cancel;
        private Task messageTask;
        private Task iisPingTask;

        private static readonly object iisPingTaskKey = new Object();

        public override void Run()
        {
            this.cancel = new CancellationTokenSource();
            messageTask = Task.Factory.StartNew(() => StartMessageTask(cancel.Token),TaskCreationOptions.LongRunning);
            iisPingTask = Task.Factory.StartNew(() => StartIisPingTask(cancel.Token), TaskCreationOptions.LongRunning);

            Task [] tasks = new Task [] { messageTask, iisPingTask };
            Task.WaitAll(tasks);
        }

        private static void StartIisPingTask(CancellationToken token)
        {
            AppPoolPinger pinger = new AppPoolPinger();
            while (!token.IsCancellationRequested)
            {
                pinger.PingGooeyCmsSites();
                pinger.Ping("http://" + GooeyConfigManager.AdminSiteHost);
                pinger.Ping(GooeyConfigManager.StoreSiteHost);

                lock (iisPingTaskKey)
                {
                    Monitor.Wait(iisPingTaskKey, TimeSpan.FromMinutes(5));
                }
            }
        }

        private static void StartMessageTask(CancellationToken token)
        {
            PageRoleWorker worker = new PageRoleWorker();
            while (!token.IsCancellationRequested)
            {
                worker.ProcessMessages();
                Thread.Sleep(100);
            }
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
