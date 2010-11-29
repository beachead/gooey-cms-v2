using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Paypal;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;

namespace Gooeycms.Webrole.Ecommerce.signup
{
    public partial class Activate : System.Web.UI.Page
    {
        private String token = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            token = Request.QueryString["token"];
            if (!Page.IsPostBack)
            {
                //Activate all of the billing agreements
                if (token != null)
                    ActivateSubscription();
            }
        }

        protected void ActivateSubscription()
        {
            try
            {
                PaypalExpressCheckout checkout = new PaypalExpressCheckout(token);
                String subscriptionId = checkout.SubscriptionId;

                Registration registration = Registrations.Load(subscriptionId);
                if (registration == null)
                    throw new ArgumentException("The registration guid is not valid and may not be used");

                CmsSubscription subscription = SubscriptionManager.GetSubscription(registration.Guid);
                if (subscription == null)
                {
                    PaypalExpressCheckout.ProfileResultStatus status = checkout.CreateRecurringPayment(registration);

                    //If there were no errors then activate the subscription
                    Registrations.ConvertToAccount(registration);

                    //Store the profile ids associated with each of these subscriptions
                    subscription = SubscriptionManager.GetSubscription(registration.Guid);
                    subscription.PaypalProfileId = status.ProfileId;
                    SubscriptionManager.Save(subscription);
                }

                PaypalProfileId.Text = subscription.PaypalProfileId;
                this.ActivateViews.SetActiveView(this.SuccessView);
            }
            catch (Exception e)
            {
                this.LblErrorReason.Text = e.Message;
                this.ActivateViews.SetActiveView(this.FailureView);
            }
        }
    }
}