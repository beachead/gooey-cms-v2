using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Constants;
using Gooeycms.Business;
using System.Text;

namespace Gooeycms.webrole.website
{
    public partial class SignupReview : System.Web.UI.Page
    {
        public String ReturnUrl;
        public String Guid;
        public Double PaypalCost;
        public String PaypalDescription;

        protected void Page_Load(object sender, EventArgs e)
        {
            ReturnUrl = Page.ResolveUrl(Request.Url + "&success=1");
            Guid = Request.QueryString["g"];

            Registration registration = Registrations.Load(this.Guid);
            CmsSubscriptionPlan plan = SubscriptionManager.GetSubscriptionPlan(registration);

            this.Firstname.Text = registration.Firstname;
            this.Lastname.Text = registration.Lastname;
            this.Email.Text = registration.Email;
            this.Company.Text = registration.Company;
            this.Subdomain.Text = registration.Sitename + GooeyConfigManager.DefaultCmsDomain;
            this.SubscriptionPlan.Text = SubscriptionManager.GetDescription(registration);

            if (plan.Price > 0)
            {
                PaypalDescription = SubscriptionManager.GetShortDescription("Gooey CMS -", registration);
                PaypalCost = SubscriptionManager.CalculateCost(registration);
                this.PaypayPanel.Visible = true;
                this.FreePanel.Visible = false;
            }
            else
            {
                this.PaypayPanel.Visible = false;
                this.FreePanel.Visible = true;
            }

            if (SubscriptionProcessorFactory.Instance.GetSubscriptionProcessor().IsDebug)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("custom=" + registration.Guid);
                builder.Append("&txn_id=" + new Random().Next(0, 100));
                builder.Append("&txn_type=subscr_signup");
                builder.Append("&subscr_id=" + new Random().Next(2000, 3000));

                this.SkipPaypal.NavigateUrl = "subscription.handler?" + builder.ToString();
                this.DebugPanel.Visible = true;
            }
        }

        protected void BtnSubscribe_Click(object sender, EventArgs e)
        {
        }
    }
}
