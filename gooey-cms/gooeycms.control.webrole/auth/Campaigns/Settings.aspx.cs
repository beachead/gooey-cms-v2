using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Util;
using gooeycms.business.salesforce;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Settings : System.Web.UI.Page
    {
        protected String SelectedPanel = "analytics-panel";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentSite.Subscription.IsCampaignEnabled)
                Response.Redirect("~/auth/default.aspx?addon=campaigns", true);

            Master.SetTitle("Campaign Settings");
            if (!Page.IsPostBack)
            {
                if (CurrentSite.Configuration.IsGoogleAnalyticsEnabled)
                {
                    this.RdoGoogleEnabledNo.Checked = false;
                    this.RdoGoogleEnabledYes.Checked = true;
                }
                else
                {
                    this.RdoGoogleEnabledNo.Checked = true;
                    this.RdoGoogleEnabledYes.Checked = false;
                }
                this.TxtGoogleAccountId.Text = CurrentSite.Configuration.GoogleAccountId;

                LoadSalesforceInfo();
            }
        }

        protected void LoadSalesforceInfo()
        {
            Boolean isEnabled = CurrentSite.Subscription.IsSalesforceEnabled;
            if (isEnabled)
            {
                if (CurrentSite.Configuration.Salesforce.IsEnabled)
                {
                    this.RdoSalesforceEnabledYes.Checked = true;
                    this.RdoSalesforceEnabledNo.Checked = false;
                }
                else
                {
                    this.RdoSalesforceEnabledYes.Checked = false;
                    this.RdoSalesforceEnabledNo.Checked = true;
                }

                String username = CurrentSite.Configuration.Salesforce.Username;
                String password = CurrentSite.Configuration.Salesforce.Password;
                String token = CurrentSite.Configuration.Salesforce.Token;

                this.TxtSalesforceUsername.Text = username;
                this.RequiredFieldValidator1.Enabled = false;
                this.TxtSalesforceToken.Text = token;

                //Try to login to salesforce to validate the account
                this.LstSalesforceAvailableFields.Items.Clear();
                SalesforcePartnerClient client = new SalesforcePartnerClient();
                try
                {
                    client.Login(username, password + token);

                    IList<SalesforcePartnerClient.LeadField> fields = client.GetAvailableLeadFields();
                    this.LblSalesforceAuthenticated.Text = "True";

                    foreach (SalesforcePartnerClient.LeadField field in fields)
                    {
                        ListItem item = new ListItem(field.ApiName + " (" + field.Label + ")", field.ApiName);
                        this.LstSalesforceAvailableFields.Items.Add(item);
                    }
                }
                catch (LoginException ex)
                {
                    this.LblSalesforceAuthenticated.Text = "False (" + ex.Message + ")";
                }
                finally
                {
                    client.Logout();
                }

                this.LstCustomMappings.Items.Clear();
                IDictionary<String, String> custom = CurrentSite.Configuration.Salesforce.CustomFieldMappings;
                foreach (String mapping in custom.Keys)
                {
                    String line = custom[mapping] + " --> " + mapping;
                    ListItem item = new ListItem(line, mapping);
                    this.LstCustomMappings.Items.Add(item);
                }
            }
        }

        protected void LstSalesforceAvailableFields_Changed(Object sender, EventArgs e)
        {
            String sfValue = this.LstSalesforceAvailableFields.SelectedValue;
            this.TxtSalesforceField.Text = sfValue;
        }

        protected void BtnAddCustomMapping_Click(Object sender, EventArgs e)
        {
            CurrentSite.Configuration.Salesforce.AddCustomFieldMapping(this.TxtSalesforceField.Text, this.TxtSalesforceFriendly.Text);

            this.LoadSalesforceInfo();
            SelectedPanel = "salesforce-panel";
        }

        protected void BtnRemoveCustomMapping_Click(Object sender, EventArgs e)
        {
            CurrentSite.Configuration.Salesforce.RemoveCustomFieldMapping(this.LstCustomMappings.SelectedValue);

            this.LoadSalesforceInfo();
            SelectedPanel = "salesforce-panel";
        }

        protected void BtnSaveLogin_Click(object sender, EventArgs e)
        {
            CurrentSite.Configuration.Salesforce.Username = this.TxtSalesforceUsername.Text;
            if (!String.IsNullOrWhiteSpace(this.TxtSalesforcePassword.Text))
                CurrentSite.Configuration.Salesforce.Password = this.TxtSalesforcePassword.Text;
            CurrentSite.Configuration.Salesforce.Token = this.TxtSalesforceToken.Text;
            CurrentSite.Configuration.Salesforce.IsEnabled = this.RdoSalesforceEnabledYes.Checked;

            this.LoadSalesforceInfo();
            SelectedPanel = "salesforce-panel";
        }

        protected void BtnSaveGoogle_Click(object sender, EventArgs e)
        {
            Boolean isEnabled = (this.RdoGoogleEnabledYes.Checked);
            String accountId = (this.TxtGoogleAccountId.Text);

            CurrentSite.Configuration.GoogleAccountId = accountId;
            CurrentSite.Configuration.IsGoogleAnalyticsEnabled = isEnabled;

            SelectedPanel = "analytics-panel";
        }
    }
}