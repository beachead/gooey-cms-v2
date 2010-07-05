using System.Linq;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;

namespace gooeycms.webrole.ecommerce
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            DiagnosticMonitorConfiguration diagObj = DiagnosticMonitor.GetDefaultInitialConfiguration();
            diagObj.Logs.ScheduledTransferLogLevelFilter = LogLevel.Information;
            diagObj.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);

            DiagnosticMonitor.Start("DiagnosticsConnectionString", diagObj);

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
