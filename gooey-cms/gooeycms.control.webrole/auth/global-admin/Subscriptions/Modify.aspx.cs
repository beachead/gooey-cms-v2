using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Paypal;
using Gooeycms.Business.Membership;
using System.Text;

namespace Gooeycms.Webrole.Control.auth.global_admin.Subscriptions
{
    public partial class Modify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<CmsSubscriptionPlan> plans = SubscriptionManager.GetSubscriptionPlans();
                foreach (CmsSubscriptionPlan plan in plans)
                {
                    ListItem item = new ListItem(plan.Name, plan.SKU);
                    this.LstSubscriptionPlans.Items.Add(item);
                }
                LoadExisting();
            }
        }

        protected void LoadExisting()
        {
            String guid = Request.QueryString["g"];
            String profileId = Request.QueryString["p"];
            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);

            if (String.IsNullOrEmpty(profileId))
                profileId = subscription.PaypalProfileId;

            PaypalProfileInfo profile = PaypalManager.Instance.GetProfileInfo(profileId);
            if (profile != null)
            {
                String s = "s";
                if (profile.TrialCyclesRemaining == 1)
                    s = "";

                this.TxtPaypalProfile.Text = profile.ProfileId;
                this.BtnExtendTrialPeriod.Visible = profile.IsTrialPeriod;
                this.LblTrialPeriodRemaining.Text = profile.IsTrialPeriod + " (" + profile.TrialCyclesRemaining.ToString() + " month" + s + " remaining)";
                this.LblCreated.Text = profile.Created.ToString("MM/dd/yyyy hh:mm:ss");
                this.LblProfileStatus.Text = "Subscription Active: " + !subscription.IsDisabled + "&nbsp;&nbsp;&nbsp;&nbsp;Paypal Profile: " + profile.Status;
                this.LblNormalAmt.Text = profile.NormalPaymentAmt.Value.ToString("c");

                if (profile.NextBillDate.HasValue)
                    this.LblNextBilling.Text = profile.NextBillDate.Value.ToString("MM/dd/yyyy");
                if (profile.NextBillAmount.HasValue)
                    this.LblNextBillingAmount.Text = profile.NextBillAmount.Value.ToString("c");

                if (profile.LastBillDate.HasValue)
                    this.LblLastBilling.Text = profile.LastBillDate.Value.ToString("MM/dd/yyyy");
                if (profile.LastBillAmount.HasValue)
                    this.LblLastBillingAmount.Text = profile.LastBillAmount.Value.ToString("c");
            }
            else
            {
                this.LblProfileStatus.Text = subscription.IsDisabled ? "Disabled (No Paypal Profile)" : "Enabled (No Paypal Profile)";
            }

            this.LblGuid.Text = guid;
            this.ChkCampaigns.Checked = subscription.IsCampaignEnabled;
            this.ChkSalesforceOption.Checked = subscription.IsSalesforceEnabled;

            this.LstSubscriptionPlans.SelectedValue = subscription.SubscriptionPlanSku;
            this.LblProductionDomain.Text = subscription.Domain;
            this.LblStagingDomain.Text = subscription.StagingDomain;

            this.BtnEnableDisable.Text = (subscription.IsDisabled) ? "Enable" : "Disable";
            this.BtnEnableDisable.OnClientClick = (subscription.IsDisabled) ? "" : "return confirm('Are you sure you want to disable this subscription?');";
        }

        protected void BtnUpdatePaypalProfile_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            
            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);
            subscription.PaypalProfileId = this.TxtPaypalProfile.Text;
            SubscriptionManager.Save(subscription);

            this.LoadExisting();
        }

        protected void BtnUpdateSubscriptionPlan_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];

            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);
            double originalCost = SubscriptionManager.CalculateCost(subscription);

            subscription.SubscriptionPlan = SubscriptionManager.GetSubscriptionPlan(this.LstSubscriptionPlans.SelectedValue);
            subscription.IsSalesforceEnabled = this.ChkSalesforceOption.Checked;
            subscription.IsCampaignEnabled = this.ChkCampaigns.Checked;

            SubscriptionManager.UpdateBillingAgreement(originalCost, subscription);

            this.LoadExisting();
        }

        protected void BtnEnableDisable_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];

            String action;
            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);
            if (subscription.IsDisabled)
            {
                SubscriptionManager.EnableSubscription(subscription);
                action = "enabled";
            }
            else
            {
                SubscriptionManager.DisableSubscription(subscription);
                action = "disabled";
            }

            this.LoadExisting();
            this.LblStatus.Text = "This subscription has been successfully " + action;
        }

        protected void BtnExtendTrialPeriod_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);

            String message;
            try
            {
                SubscriptionManager.ExtendTrialPeriod(subscription,1);
                message = "Successfully extended the trial period for the subscription";
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }
            this.LoadExisting();
            this.LblStatus.Text = message;
        }

        protected void BtnDelete_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);

            String profileId = subscription.PaypalProfileId;
            SubscriptionManager.CancelSubscription(subscription);

            PaypalExpressCheckout paypal = new PaypalExpressCheckout();
            PaypalProfileInfo info = paypal.GetProfileInfo(profileId);

            if (info != null)
                this.LblProfileStatus.Text = info.Status;
            else
                this.LblProfileStatus.Text = "DELETED";

            this.EditPanel.Enabled = false;
            this.LblStatus.Text = "This subscription has been successfully deleted.";
        }
    }
}