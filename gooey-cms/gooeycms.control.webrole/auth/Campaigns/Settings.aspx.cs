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
                String username = CurrentSite.Configuration.Salesforce.Username;
                String password = CurrentSite.Configuration.Salesforce.Password;
                String token = CurrentSite.Configuration.Salesforce.Token;

                this.TxtSalesforceUsername.Text = username;
                this.TxtSalesforcePassword.Text = "nottheactualpassword";
                this.TxtSalesforceToken.Text = token;

                //Try to login to salesforce to validate the account
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

            }
        }

        protected void BtnSaveLogin_Click(object sender, EventArgs e)
        {
            CurrentSite.Configuration.Salesforce.Username = this.TxtSalesforceUsername.Text;
            if (!String.IsNullOrWhiteSpace(this.TxtSalesforcePassword.Text))
                CurrentSite.Configuration.Salesforce.Password = this.TxtSalesforcePassword.Text;
            CurrentSite.Configuration.Salesforce.Token = this.TxtSalesforceToken.Text;

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