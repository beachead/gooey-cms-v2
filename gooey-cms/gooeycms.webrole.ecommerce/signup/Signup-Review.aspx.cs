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
using Gooeycms.Business.Paypal;

namespace Gooeycms.Webrole.Ecommerce
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

            PaypalDescription = SubscriptionManager.GetShortDescription("Gooey CMS -", registration);
            PaypalCost = SubscriptionManager.CalculateCost(registration);

            if (GooeyConfigManager.IsPaypalSandbox)
                BtnSubscribe.OnClientClick = "alert('This purchase is using the paypal sandbox environment. No actual funds will be transferred.')";
        }

        protected void BtnSubscribe_Click(Object sender, EventArgs e)
        {
            Registration registration = Registrations.Load(this.Guid);

            //Create all of the billing agreements
            PaypalExpressCheckout checkout = new PaypalExpressCheckout();
            checkout.AddBillingAgreement(registration);
            
            String redirect = checkout.SetExpressCheckout(registration.Email, registration.Guid);
            Response.Redirect(redirect, true);
        }

    }
}
