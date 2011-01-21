using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Util;
using Gooeycms.Business.Paypal;
using Gooeycms.Data.Model.Billing;
using Gooeycms.Business.Billing;
using System.Text;
using Gooeycms.Business.Membership;
using Gooeycms.Business;
using Gooeycms.Business.Email;
using System.Globalization;
using Gooeycms.Business.Campaigns;
using Gooeycms.Constants;
using Gooeycms.Extensions;
using Gooeycms.Business.Crypto;

namespace Gooeycms.Webrole.Control.auth
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LblStatus.Text = "";

            if (!CurrentSite.IsAvailable)
                Response.Redirect("~/Dashboard.aspx");

            String token = Request.QueryString["token"];
            if (!String.IsNullOrEmpty(token))
            {
                PaypalExpressCheckout checkout = new PaypalExpressCheckout(token);
                String subscriptionId = checkout.SubscriptionId;

                CmsSubscription subscription = SubscriptionManager.GetSubscription(subscriptionId);
                subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Business);

                int freeTrialRemaining = (int)SubscriptionManager.CalculateFreeTrialRemaining(subscription);

                //Get the options cookie
                Boolean foundCookie = false;
                HttpCookie cookie = Request.Cookies["upgrade-options"];
                if (cookie != null)
                {
                    try
                    {
                        String[] arr = TextEncryption.Decode(cookie.Value).Split('|');
                        if (arr.Length == 2)
                        {
                            subscription.IsCampaignEnabled = (arr[0].Equals("true")) ? true : false;
                            subscription.IsSalesforceEnabled = (arr[1].Equals("true")) ? true : false;

                            foundCookie = true;
                        }
                    }
                    catch (Exception) { }
                }

                if (!foundCookie)
                {
                    Response.Redirect("Manage.aspx?upgrade=failure&type=missingcookie", true);
                }


                Gooeycms.Business.Paypal.PaypalExpressCheckout.ProfileResultStatus status = checkout.CreateRecurringPayment(subscription, freeTrialRemaining);
                subscription.PaypalProfileId = status.ProfileId;
                subscription.IsDisabled = false; //Always make sure to reenable the subscription
                subscription.MaxPhoneNumbers = -1; //Set back to the default value
                SubscriptionManager.Save(subscription);

                //Clear the site cache
                CurrentSite.Cache.Clear();

                BillingManager.Instance.AddHistory(subscription.Guid, subscription.PaypalProfileId, null, BillingManager.Upgrade,0,"Successfully upgrade subscription or modified subscription options and created paypal recurring payment profile: " + subscription.PaypalProfileId);
                Response.Redirect("Manage.aspx?upgrade=success", true);
            }

            if (!Page.IsPostBack)
            {
                LoadInfo();
            }

            if (Request.QueryString["upgrade"] != null)
            {
                this.LblStatus.Text = "Your account was successfully upgraded!";
                this.LblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void LoadInfo()
        {
            this.LblUsername.Text = LoggedInUser.Username;
            this.TxtFirstname.Text = LoggedInUser.Wrapper.UserInfo.Firstname;
            this.TxtLastname.Text = LoggedInUser.Wrapper.UserInfo.Lastname;
            this.TxtCompany.Text = LoggedInUser.Wrapper.UserInfo.Company;

            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);

            List<CultureInfo> cultures = new List<CultureInfo>(System.Globalization.CultureInfo.GetCultures(CultureTypes.SpecificCultures));
            foreach (CultureInfo culture in cultures.OrderBy(c => c.EnglishName))
            {
                this.LstSiteCulture.Items.Add(new ListItem(culture.EnglishName, culture.Name.ToLower()));
            }
            this.LstSiteCulture.SelectedValue = subscription.Culture;

            this.LblDomain.Text = subscription.DefaultDisplayName;

            this.ChkSalesforce.Visible = true;
            if (subscription.SubscriptionPlanEnum == Constants.SubscriptionPlans.Free)
            {
                this.UpgradeOptions.SetActiveView(this.UpgradeAvailable);

                //Check if this user has campaigns, if so, check campaigns by default
                this.ChkUpgradeCampaignOption.Checked = subscription.IsCampaignEnabled;
                
                //Calculate how long the user has left in their free trial period
                double daysLeft = SubscriptionManager.CalculateFreeTrialRemaining(CurrentSite.Subscription);
                DateTime firstBilling = DateTime.Now.AddDays(daysLeft);
                this.LblBillingStartDate.Text = String.Format("{0:MMMMM dd, yyyy}", firstBilling);

                double price = (double)SubscriptionManager.GetSubscriptionPlan(SubscriptionPlans.Business).Price;
                this.LblSubscriptionPrice.Text = String.Format("{0:c}",price);
                this.LblCampaignPrice.Text = String.Format("{0:c} per month",GooeyConfigManager.CampaignOptionPrice);
                this.LblSalesforcePrice.Text = String.Format("{0:c} per month", GooeyConfigManager.SalesForcePrice);

                if (this.ChkUpgradeCampaignOption.Checked)
                    price += GooeyConfigManager.CampaignOptionPrice;

                this.LblTotalAmount.Text = String.Format("{0:c}", price);
                this.ChkSalesforce.Visible = false;
            }
            else
                this.UpgradeOptions.SetActiveView(this.DowngradeAvailable);

            CmsSubscriptionPlan currentPlan = subscription.SubscriptionPlan;

            /* Configure the upgrade subscription price, based upon the current subscription configuration */
            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Business);
            StringBuilder builder = SubscriptionManager.GetSubscriptionDescription(subscription);
            String msg = String.Format("Are you sure you want to upgrade to the {0}?",builder.ToString());

            subscription.SubscriptionPlan = currentPlan;

            PaypalProfileInfo paypal = null;
            if (!String.IsNullOrEmpty(subscription.PaypalProfileId))
            {
                paypal = PaypalManager.Instance.GetProfileInfo(subscription.PaypalProfileId);
                if (paypal.NextBillDate.HasValue)
                    this.LblNextBillingDate.Text = paypal.NextBillDate.Value.ToLongDateString();
                this.LblPaypalBillingId.Text = paypal.ProfileId;
                this.LblPaypalStatus.Text = paypal.Status;

                if (!paypal.IsCancelled)
                    this.LblPlanCost.Text = String.Format("{0:c}", paypal.NormalPaymentAmt);
                else
                    this.LblPlanCost.Text = String.Format("{0:c}", 0);

                if (subscription.IsCampaignEnabled)
                    this.ChkCampaigns.Checked = true;
                if (subscription.IsSalesforceEnabled)
                    this.ChkSalesforce.Checked = true;
                this.PanelBusinessPlan.Visible = true;

                this.BtnUpdateOptions.Visible = true;
                this.LblTrialRemaining.Text = paypal.TrialCyclesRemaining.ToString();

                if (paypal.IsTrialPeriod)
                    this.PaypalTrialView.SetActiveView(this.InTrialPeriodView);
                else
                    this.PaypalTrialView.SetActiveView(this.OutOfTrialPeriodView);

                if (paypal.IsCancelled)
                {
                    this.PanelBusinessPlan.Enabled = false;
                    this.BtnUpdateOptions.Visible = false;
                }
            }
            else
            {
                this.PanelBusinessPlan.Visible = false;
                this.LblPlanCost.Text = String.Format("{0:c}", 0);
            }

            if (subscription.IsDisabled)
            {
                this.ManagePanel.Enabled = false;
                this.ManageEnablePanel.Visible = true;

                if (paypal != null)
                {
                    if (paypal.IsCancelled)
                        this.EnableStatusView.SetActiveView(CancelledView);
                    else
                        this.EnableStatusView.SetActiveView(SuspendedView);
                }
                else
                {
                    this.EnableStatusView.SetActiveView(FreeView);
                }
            }
            else
            {
                this.ManagePanel.Enabled = true;
                this.ManageEnablePanel.Visible = false;
            }

            this.LblCurrentPlan.Text = subscription.SubscriptionPlan.Name;
            this.LblSiteName.Text = subscription.Subdomain;
            this.TxtProductionDomain.Text = subscription.Domain;
            this.TxtCustomStagingDomain.Text = subscription.StagingDomain;
        }

        protected void RecalculateCost_Click(Object sender, EventArgs e)
        {
            double total = (double)SubscriptionManager.GetSubscriptionPlan(SubscriptionPlans.Business).Price;
            if (this.ChkUpgradeCampaignOption.Checked)
                total += GooeyConfigManager.CampaignOptionPrice;
            if (this.ChkUpgradeSalesforceOption.Checked)
                total += GooeyConfigManager.SalesForcePrice;

            this.LblTotalAmount.Text = String.Format("{0:c}", total);
        }

        protected void BtnUpdateUserInfo_Click(Object sender, EventArgs e)
        {
            UserInfo info = LoggedInUser.Wrapper.UserInfo;
            info.Firstname = this.TxtFirstname.Text;
            info.Lastname = this.TxtLastname.Text;
            info.Company = this.TxtCompany.Text;

            MembershipUtil.UpdateUserInfo(info);

            this.LblStatus.Text = "Successfully updated account information";
            this.LblStatus.ForeColor = System.Drawing.Color.Green;
        }

        protected void LnkUpdateDomain_Click(Object sender, EventArgs e)
        {
            try
            {
                CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);

                String production = this.TxtProductionDomain.Text.Trim();
                String staging = this.TxtCustomStagingDomain.Text.Trim();

                if (String.IsNullOrWhiteSpace(production))
                    production = subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;
                if (String.IsNullOrWhiteSpace(staging))
                    staging = GooeyConfigManager.DefaultStagingPrefix + subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;

                SubscriptionManager.UpdateDomains(subscription, production, staging, true);

                //Clear the site cache
                CurrentSite.Cache.Clear();


                this.LblStatus.Text = "Successfully updated domains";
                this.LblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                this.LblStatus.Text = ex.Message;
                this.LblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void BtnCancelPlan_Click(Object sender, EventArgs e)
        {
            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            SubscriptionManager.DisableSubscription(subscription);

            //Clear the site cache
            CurrentSite.Cache.Clear();

            EmailManager.Instance.SendCancellationEmail(subscription);

            Response.Redirect("~/auth/dashboard.aspx?g=" + subscription.Guid + "&disabled=true",true);
        }

        protected void BtnUpdateOptions_Click(Object sender, EventArgs e)
        {
            try
            {
                CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);

                Boolean campaignsEnabled = this.ChkCampaigns.Checked;
                Boolean salesforceEnabled = this.ChkSalesforce.Checked;

                SubscriptionManager.UpdateSubscriptionOptions(subscription, campaignsEnabled, salesforceEnabled);

                //Clear the site cache
                CurrentSite.Cache.Clear();

                this.LoadInfo();
            }
            catch (Exception ex)
            {
                this.LblStatus.Text = ex.Message;
                this.LblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void BtnEnableSubscription_Click(Object sender, EventArgs e)
        {
            String returnurl = "http://" + GooeyConfigManager.AdminSiteHost + "/auth/Manage.aspx";
            String cancelurl = returnurl;

            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            
            if (subscription.SubscriptionPlanEnum == Constants.SubscriptionPlans.Business) 
            {
                PaypalExpressCheckout checkout = new PaypalExpressCheckout();
                PaypalProfileInfo paypal = PaypalManager.Instance.GetProfileInfo(subscription.PaypalProfileId);
                if (paypal.IsSuspended)
                {
                    //Simply re-enable the suspended profile
                    checkout.Reactivate(subscription.PaypalProfileId);
                }
                else if (paypal.IsCancelled)
                {
                    //Send the user to paypal to reestablish the billing profile
                    BtnUpgradePlan_Click(sender, e);
                }
            }

            subscription.IsDisabled = false;
            SubscriptionManager.Save(subscription);

            //Clear the site cache
            CurrentSite.Cache.Clear();

            Response.Redirect("~/auth/Manage.aspx");
        }

        protected void BtnUpgradePlan_Click(Object sender, EventArgs e)
        {
            String returnurl = "http://" + GooeyConfigManager.AdminSiteHost + "/auth/Manage.aspx";
            String cancelurl = returnurl;

            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            subscription.IsCampaignEnabled = false;
            subscription.IsSalesforceEnabled = false;

            if (this.ChkUpgradeCampaignOption.Checked)
                subscription.IsCampaignEnabled = true;

            if (this.ChkUpgradeSalesforceOption.Checked)
                subscription.IsSalesforceEnabled = true;

            String options = String.Format("{0}|{1}",subscription.IsCampaignEnabled.StringValue(), subscription.IsSalesforceEnabled.StringValue());
            options = TextEncryption.Encode(options);

            int freeTrialLength = (int)SubscriptionManager.CalculateFreeTrialRemaining(subscription);

            //Cookie the user with what options need enabled
            HttpCookie cookie = new HttpCookie("upgrade-options",options);
            Response.Cookies.Add(cookie);

            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Business);

            PaypalExpressCheckout checkout = new PaypalExpressCheckout();
            checkout.AddBillingAgreement(PaypalExpressCheckout.GetBillingAgreement(subscription,freeTrialLength));
            String redirect = checkout.SetExpressCheckout(LoggedInUser.Email, subscription.Guid,returnurl,cancelurl);

            Response.Redirect(redirect, true);
        }

        protected void BtnDowngradePlan_Click(Object sender, EventArgs e)
        {
            //Cancel the paypal subscription
            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            subscription.IsCampaignEnabled = false;
            subscription.IsSalesforceEnabled = false;

            PaypalExpressCheckout checkout = new PaypalExpressCheckout();
            checkout.Cancel(subscription.PaypalProfileId);

            String paypalId = subscription.PaypalProfileId;
            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Free);
            SubscriptionManager.Save(subscription);

            //Clear the site cache
            CurrentSite.Cache.Clear();

            BillingManager.Instance.AddHistory(subscription.Guid, paypalId, null, BillingManager.Downgrade, 0, "Paypal billing profile successfully cancelled and subscription downgraded to 'free'");

            LoadInfo();
        }
    }
}