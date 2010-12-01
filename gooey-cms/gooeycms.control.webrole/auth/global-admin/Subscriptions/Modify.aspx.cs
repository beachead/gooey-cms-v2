using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Paypal;

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

                if (profile.NextBillDate.HasValue)
                    this.LblNextBilling.Text = profile.NextBillDate.Value.ToString("MM/dd/yyyy");
                if (profile.NextBillAmount.HasValue)
                    this.LblNextBillingAmount.Text = profile.NextBillAmount.Value.ToString("c");

                if (profile.LastBillDate.HasValue)
                    this.LblLastBilling.Text = profile.LastBillDate.Value.ToString("MM/dd/yyyy");
                if (profile.LastBillAmount.HasValue)
                    this.LblLastBillingAmount.Text = profile.LastBillAmount.Value.ToString("c");
            }

            this.LblGuid.Text = guid;
            this.ChkCampaigns.Checked = subscription.IsCampaignEnabled;
            this.ChkSalesforceOption.Checked = subscription.IsSalesforceEnabled;

            this.LstSubscriptionPlans.SelectedValue = subscription.SubscriptionPlanSku;
            this.LblProductionDomain.Text = subscription.Domain;
            this.LblStagingDomain.Text = subscription.StagingDomain;
        }
    }
}