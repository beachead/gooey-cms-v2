using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Campaigns;
using Gooeycms.Data.Model.Form;
using Gooeycms.Constants;
using Gooeycms.Business.Forms;
using gooeycms.business.salesforce;
using Gooeycms.Business.Util;
using Gooeycms.Business.Subscription;

namespace Gooeycms.Business.Twilio.Handlers
{
    public class ForwardInitHandler : BaseTwilioHandler
    {
        private const String DefaultValue = "<phone-lead>";
        protected override void WriteInstructions(System.Web.HttpContext context, StringBuilder instructions)
        {
            this.InsertDatabaseLead();
            this.InsertSalesforceLead();
            
            String forwardNumber = base.GetAssociatedPhoneNumber().GetForwardNumber();
            StringBuilder builder = new StringBuilder();
            if (forwardNumber == null)
            {
                instructions.AppendLine("<Say>We are sorry but this company has not configured a forwarding number. We are unable to connect your call</Say>");
            }
            else
            {
                instructions.AppendFormat("<Say>We are connecting your call</Say>").AppendLine();
                instructions.AppendFormat("<Dial>{0}</Dial>",forwardNumber).AppendLine();
            }
        }

        private void InsertSalesforceLead()
        {
            Boolean enabled = false;
            String temp = base.GetSiteConfigurationValue("salesforce-enabled");
            Boolean.TryParse(temp, out enabled);

            if (CurrentSite.Configuration.Salesforce.IsEnabled)
            {
                String username = CurrentSite.Configuration.Salesforce.Username;
                String password = CurrentSite.Configuration.Salesforce.Password; 
                String token = CurrentSite.Configuration.Salesforce.Token;

                String apiPassword = password + token;
                SalesforcePartnerClient client = new SalesforcePartnerClient();

                String firstname = base.CallName;
                String lastname = "";

                String[] arr = base.CallName.Split(' ');
                if (arr.Length >= 2)
                {
                    firstname = arr[1];
                    lastname = arr[0];
                }

                Dictionary<String, String> fields = new Dictionary<string, string>();
                fields.Add("Phone", base.CallFromNumber);
                fields.Add("FirstName", firstname);
                fields.Add("LastName", lastname);
                fields.Add("PostalCode", base.CallFromZip);
                fields.Add("City", base.CallFromCity);
                fields.Add("State", base.CallFromState);
                fields.Add("Description", DateTime.Now + Environment.NewLine + "Customer call received from " + CurrentSite.ProductionDomain + Environment.NewLine + "Campaign: " + base.GetAssociatedCampaign().Name + " (" + base.GetAssociatedCampaign().TrackingCode + ")");

                try
                {
                    client.Login(username,apiPassword);
                    client.AddLead(fields, CurrentSite.Configuration.Salesforce.CustomFieldMappings);
                    client.Logout();
                }
                catch (Exception)
                { }
            }
        }

        private void InsertDatabaseLead()
        {
            StringBuilder keys = new StringBuilder();
            StringBuilder values = new StringBuilder();

            keys.Append("from-number").Append(CmsForm.FieldSeparator);
            values.Append(base.CallFromNumber).Append(CmsForm.FieldSeparator);

            keys.Append("from-city").Append(CmsForm.FieldSeparator);
            values.Append(base.CallFromCity).Append(CmsForm.FieldSeparator);

            keys.Append("from-state").Append(CmsForm.FieldSeparator);
            values.Append(base.CallFromState).Append(CmsForm.FieldSeparator);

            keys.Append("from-zip").Append(CmsForm.FieldSeparator);
            values.Append(base.CallFromZip).Append(CmsForm.FieldSeparator);

            keys.Append("from-name").Append(CmsForm.FieldSeparator);
            values.Append(base.CallName).Append(CmsForm.FieldSeparator);

            //First, track that a lead has come in from this phone number
            CmsForm form = new CmsForm();
            form.Guid = System.Guid.NewGuid().ToString();
            form.SubscriptionId = base.GetAssociatedPhoneNumber().SubscriptionId;
            form.DownloadedFile = DefaultValue;
            form.Email = DefaultValue;
            form.IpAddress = DefaultValue;
            form.RawCampaigns = base.GetAssociatedCampaign().TrackingCode;
            form.FormUrl = DefaultValue;
            form.Inserted = DateTime.Now;
            form.IsPhoneLead = true;

            form._FormKeys = keys.ToString();
            form._FormValues = values.ToString();
            FormManager.Instance.Save(form);
        }
    }
}
