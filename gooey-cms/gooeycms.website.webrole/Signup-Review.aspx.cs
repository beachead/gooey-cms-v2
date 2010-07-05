using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;
using gooeycms.constants;
using Gooeycms.Business;

namespace gooeycms.webrole.website
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
            CmsSubscriptionPlan plan = Subscriptions.GetSubscriptionPlan(registration);

            this.Firstname.Text = registration.Firstname;
            this.Lastname.Text = registration.Lastname;
            this.Email.Text = registration.Email;
            this.Company.Text = registration.Company;
            this.Subdomain.Text = registration.Sitename + GooeyConfigManager.DefaultCmsDomain;
            this.SubscriptionPlan.Text = Subscriptions.GetDescription(registration);

            if (plan.Price > 0)
            {
                PaypalDescription = Subscriptions.GetShortDescription("Gooey CMS -", registration);
                PaypalCost = Subscriptions.CalculateCost(registration);
                this.PaypayPanel.Visible = true;
                this.FreePanel.Visible = false;
            }
            else
            {
                this.PaypayPanel.Visible = false;
                this.FreePanel.Visible = true;
            }
        }

        protected void BtnSubscribe_Click(object sender, EventArgs e)
        {
        }
    }
}
