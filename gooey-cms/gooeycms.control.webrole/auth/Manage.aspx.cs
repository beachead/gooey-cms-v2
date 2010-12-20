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

namespace Gooeycms.Webrole.Control.auth
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentSite.IsAvailable)
                Response.Redirect("~/Dashboard.aspx");

            String token = Request.QueryString["token"];
            if (!String.IsNullOrEmpty(token))
            {
                PaypalExpressCheckout checkout = new PaypalExpressCheckout(token);
                String subscriptionId = checkout.SubscriptionId;

                CmsSubscription subscription = SubscriptionManager.GetSubscription(subscriptionId);
                subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Business);

                Gooeycms.Business.Paypal.PaypalExpressCheckout.ProfileResultStatus status = checkout.CreateRecurringPayment(subscription);
                subscription.PaypalProfileId = status.ProfileId;
                SubscriptionManager.Save(subscription);

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
            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            this.LblDomain.Text = subscription.DefaultDisplayName;

            if (subscription.SubscriptionPlanEnum == Constants.SubscriptionPlans.Free)
            {
                this.BtnDowngradePlan.Visible = false;
                this.BtnUpgradePlan.Enabled = true;
            }
            else
            {
                this.BtnDowngradePlan.Enabled = true;
                this.BtnUpgradePlan.Visible = false;
            }

            CmsSubscriptionPlan currentPlan = subscription.SubscriptionPlan;

            /* Configure the upgrade subscription price, based upon the current subscription configuration */
            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Business);
            StringBuilder builder = SubscriptionManager.GetSubscriptionDescription(subscription);
            String msg = String.Format("Are you sure you want to upgrade to the {0}?",builder.ToString());
            this.BtnUpgradePlan.OnClientClick = "return confirm('" + msg + "\\r\\n\\r\\nYou will be redirected to Paypal\\'s website to complete the upgrade transaction.\\r\\n\\r\\n(You will be able to modify your subscription options after your account has been upgraded.)');";

            subscription.SubscriptionPlan = currentPlan;
            if (!String.IsNullOrEmpty(subscription.PaypalProfileId))
            {
                PaypalProfileInfo paypal = PaypalManager.Instance.GetProfileInfo(subscription.PaypalProfileId);
                this.LblCurrentPlan.Text = subscription.SubscriptionPlan.Name;
                if (paypal.NextBillDate.HasValue)
                    this.LblNextBillingDate.Text = paypal.NextBillDate.Value.ToLongDateString();
                this.LblPaypalBillingId.Text = paypal.ProfileId;
                this.LblPaypalStatus.Text = paypal.Status;
                this.LblPlanCost.Text = String.Format("{0:c}",paypal.NormalPaymentAmt);

                if (subscription.IsCampaignEnabled)
                    this.ChkCampaigns.Checked = true;
                if (subscription.IsSalesforceEnabled)
                    this.ChkSalesforce.Checked = true;
                this.PanelBusinessPlan.Visible = true;

                this.BtnUpdateOptions.Visible = true;
                if (paypal.IsCancelled)
                {
                    this.PanelBusinessPlan.Enabled = false;
                    this.BtnUpdateOptions.Visible = false;
                }
            }
            else
            {
                this.PanelBusinessPlan.Visible = false;
            }

            this.LblSiteName.Text = subscription.Subdomain;
            this.TxtProductionDomain.Text = subscription.Domain;
            this.TxtCustomStagingDomain.Text = subscription.StagingDomain;
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
            }
            catch (Exception ex)
            {
                this.LblStatus.Text = ex.Message;
                this.LblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void BtnUpgradePlan_Click(Object sender, EventArgs e)
        {
            String returnurl = "http://" + GooeyConfigManager.AdminSiteHost + "/auth/Manager.aspx";
            String cancelurl = returnurl;

            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Business);

            PaypalExpressCheckout checkout = new PaypalExpressCheckout();
            checkout.AddBillingAgreement(PaypalExpressCheckout.GetBillingAgreement(subscription));
            String redirect = checkout.SetExpressCheckout(LoggedInUser.Email, subscription.Guid,returnurl,cancelurl);

            Response.Redirect(redirect, true);
        }

        protected void BtnDowngradePlan_Click(Object sender, EventArgs e)
        {
            //Cancel the paypal subscription
            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            
            PaypalExpressCheckout checkout = new PaypalExpressCheckout();
            checkout.Cancel(subscription.PaypalProfileId);

            String paypalId = subscription.PaypalProfileId;
            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(Constants.SubscriptionPlans.Free);
            SubscriptionManager.Save(subscription);

            BillingManager.Instance.AddHistory(subscription.Guid, paypalId, null, BillingManager.Downgrade, 0, "Paypal billing profile successfully cancelled and subscription downgraded to 'free'");

            LoadInfo();
        }
    }
}