using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using gooeycms.business.salesforce;

namespace Gooeycms.Business.Forms.Plugins
{
    public class SalesForcePlugin : FormPlugin
    {
        public override bool IsEnabled()
        {
            return CurrentSite.Configuration.Salesforce.IsEnabled;
        }

        public override void Process()
        {
            String username = CurrentSite.Configuration.Salesforce.Username;
            String password = CurrentSite.Configuration.Salesforce.Password;
            String token = CurrentSite.Configuration.Salesforce.Token;

            String apiPassword = password + token;

            SalesforcePartnerClient client = new SalesforcePartnerClient();
            try
            {
                client.Login(username, apiPassword);
                client.AddLead(base.FormFields);
                client.Logout();
            }
            catch (Exception e)
            {
            }
        }

        public override bool IsExceptionFatal()
        {
            return false; ;
        }
    }
}
