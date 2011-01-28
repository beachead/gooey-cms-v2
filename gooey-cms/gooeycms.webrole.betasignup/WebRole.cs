using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.Web.Administration;

namespace gooeycms.webrole.betasignup
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            /*
             * HACK: Microsoft screwed up the machine key implementation in Azure 1.3
             * therefore, we need to override the configuration and force the machine key
             * The below code does just that
             * http://msdn.microsoft.com/en-us/library/gg494983.aspx
             */
            using (var server = new ServerManager())
            {
                // This name must match the site name as defined in the ServiceDefinition.csdef
                var siteNameFromServiceModel = "Web";
                var siteName =
                    string.Format("{0}_{1}", RoleEnvironment.CurrentRoleInstance.Id, siteNameFromServiceModel);
                var siteConfig = server.Sites[siteName].GetWebConfiguration();

                // get the appSettings section
                var appSettings = siteConfig.GetSection("appSettings").GetCollection()
                    .ToDictionary(e => (string)e["key"], e => (string)e["value"]);

                // reconfigure the machine key
                var machineKeySection = siteConfig.GetSection("system.web/machineKey");
                machineKeySection.SetAttributeValue("validationKey", appSettings["validationKey"]);
                machineKeySection.SetAttributeValue("validation", appSettings["validation"]);
                machineKeySection.SetAttributeValue("decryptionKey", appSettings["decryptionKey"]);
                machineKeySection.SetAttributeValue("decryption", appSettings["decryption"]);

                server.CommitChanges();
            }

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
