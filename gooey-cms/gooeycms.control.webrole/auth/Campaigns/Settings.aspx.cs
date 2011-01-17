using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Util;
using gooeycms.business.salesforce;
using Gooeycms.Business.Twilio;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Settings : App_Code.ValidatedHelpPage
    {
        protected String SelectedPanel = "analytics-panel";

        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!CurrentSite.Subscription.IsCampaignEnabled)
                Response.Redirect("~/auth/default.aspx?addon=campaigns", true);

            Master.SetTitle("Campaign Settings");
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["pid"] != null)
                    SelectedPanel = Request.QueryString["pid"];

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
                LoadPhoneInfo();
            }
        }

        protected void LoadPhoneInfo()
        {
            this.RdoTollFreeType.Checked = false;
            this.RdoLocalType.Checked = false;

            if (CurrentSite.Configuration.PhoneSettings.NumberType == CurrentSite.Configuration.PhoneSettings.PhoneNumberType.Local)
                this.RdoLocalType.Checked = true;
            else if (CurrentSite.Configuration.PhoneSettings.NumberType == CurrentSite.Configuration.PhoneSettings.PhoneNumberType.TollFree)
                this.RdoTollFreeType.Checked = true;

            this.TxtAreaCode.Text = CurrentSite.Configuration.PhoneSettings.DefaultAreaCode;
            if (CurrentSite.Configuration.PhoneSettings.IsActive)
                this.TxtForwardNumber.Text = AvailablePhoneNumber.Parse(CurrentSite.Configuration.PhoneSettings.DefaultForwardNumber).WebDisplay;

            this.LblRemainingPhoneNumbers.Text = CurrentSite.Configuration.PhoneSettings.RemainingPhoneNumbers.ToString();
            this.TxtPhoneFormat.Text = CurrentSite.Configuration.PhoneSettings.DefaultPhoneFormat;

            this.AssignedPhoneNumbers.DataSource = CurrentSite.Configuration.PhoneSettings.GetActivePhoneNumbers();
            this.AssignedPhoneNumbers.DataBind();
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

        protected void BtnPhoneSettings_Click(Object sender, EventArgs e)
        {
            if (this.RdoLocalType.Checked)
                CurrentSite.Configuration.PhoneSettings.NumberType = CurrentSite.Configuration.PhoneSettings.PhoneNumberType.Local;
            else if (this.RdoTollFreeType.Checked)
                CurrentSite.Configuration.PhoneSettings.NumberType = CurrentSite.Configuration.PhoneSettings.PhoneNumberType.TollFree;

            CurrentSite.Configuration.PhoneSettings.DefaultPhoneFormat = this.TxtPhoneFormat.Text;

            String status = "Successfully updated phone settings for this subscription";

            this.LblPhoneStatus.ForeColor = System.Drawing.Color.Green;
            String areacode = this.TxtAreaCode.Text;
            if (!String.IsNullOrWhiteSpace(areacode))
            {
                int results = CurrentSite.Configuration.PhoneSettings.GetLocalTwilioClient().SearchAvailableLocalNumbers(areacode).Count;
                if (results > 0)
                {
                    CurrentSite.Configuration.PhoneSettings.DefaultAreaCode = areacode;
                }
                else
                {
                    status = "We're sorry, but there are not currently any phone numbers available in the " + areacode + " area code.";
                    this.LblPhoneStatus.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (!String.IsNullOrWhiteSpace(this.TxtForwardNumber.Text))
            {
                CurrentSite.Configuration.PhoneSettings.DefaultForwardNumber = AvailablePhoneNumber.Parse(this.TxtForwardNumber.Text).PhoneNumber;
            }
            else
            {
                status = "You must assign a default forwarding number to activate campaign-specific phone functionality.";
                this.LblPhoneStatus.ForeColor = System.Drawing.Color.Red;
            }

            this.LblPhoneStatus.Text = status;

            SelectedPanel = "phone-panel";
        }
    }
}