using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Paypal;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Billing;
using Gooeycms.Business.Email;

namespace Gooeycms.Webrole.Ecommerce.signup
{
    public partial class Activate : System.Web.UI.Page
    {
        private String token = null;
        private String ftoken = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            token = Request.QueryString["token"];
            ftoken = Request.QueryString["_ftoken"];
            if (!Page.IsPostBack)
            {
                //Activate all of the billing agreements
                if (token != null)
                    ActivateSubscription();
                else if (ftoken != null)
                    ActivateFreeSubscription();
            }
        }

        protected void ActivateFreeSubscription()
        {
            try
            {
                String guid = TextEncryption.Decode(ftoken);
                Registration registration = Registrations.Load(guid);
                CmsSubscription subscription = CreateSubscription(registration, null);

                this.ActivateViews.SetActiveView(this.SuccessFreeView);
            }
            catch (Exception e)
            {
                this.LblErrorReason.Text = e.Message;
                this.ActivateViews.SetActiveView(this.FailureView);
            }
        }

        protected void ActivateSubscription()
        {
            try
            {
                PaypalExpressCheckout checkout = new PaypalExpressCheckout(token);
                String subscriptionId = checkout.SubscriptionId;

                Registration registration = Registrations.Load(subscriptionId);

                PaypalExpressCheckout.ProfileResultStatus status = checkout.CreateRecurringPayment(registration);
                CmsSubscription subscription = CreateSubscription(registration, status.ProfileId);

                PaypalProfileId.Text = subscription.PaypalProfileId;
                this.ActivateViews.SetActiveView(this.SuccessPaypalView);
            }
            catch (Exception e)
            {
                this.LblErrorReason.Text = e.Message;
                this.ActivateViews.SetActiveView(this.FailureView);
            }
        }

        private CmsSubscription CreateSubscription(Registration registration, String paypalProfileId)
        {
            if (registration == null)
                throw new ArgumentException("The registration guid is not valid and may not be used");

            CmsSubscription subscription = SubscriptionManager.GetSubscription(registration.Guid);
            if (subscription == null)
            {
                //If there were no errors then activate the subscription
                Registrations.ConvertToAccount(registration);

                //Store the profile ids associated with each of these subscriptions
                subscription = SubscriptionManager.GetSubscription(registration.Guid);
                subscription.PaypalProfileId = paypalProfileId;
                SubscriptionManager.Save(subscription);

                double totalCost = SubscriptionManager.CalculateCost(subscription);
                BillingManager.Instance.AddHistory(subscription.Guid, paypalProfileId, BillingManager.NotApplicable, BillingManager.Signup, totalCost, "Initial signup for " + subscription.SubscriptionPlan.Name);
                EmailManager.Instance.SendRegistrationEmail(subscription);
            }

            return subscription;
        }
    }
}