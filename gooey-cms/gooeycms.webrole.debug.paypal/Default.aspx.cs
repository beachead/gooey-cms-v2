using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Paypal;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Constants;
using Encore.PayPal.Nvp;
using Gooeycms.Business;

namespace gooeycms.webrole.debug.paypal
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["token"] != null)
                CreateRecurringPayment();
        }

        protected void BtnCreate_Click(Object sender, EventArgs e)
        {
            String guid = System.Guid.NewGuid().ToString();
            String redirect = SetExpressCheckout("cadams@prayer-warrior.net", guid);

            Response.Redirect(redirect, true);
        }

        private void CreateRecurringPayment()
        {
            String token = Request.QueryString["token"];
            int amount = 100;
            NvpCreateRecurringPaymentsProfile action = new NvpCreateRecurringPaymentsProfile();
            SetDefaults(action);

            action.Add(NvpCreateRecurringPaymentsProfile.Request._TOKEN, token);
            action.Add(NvpCreateRecurringPaymentsProfile.Request._DESC, "This is a test subscription");
            action.Add(NvpCreateRecurringPaymentsProfile.Request._PROFILESTARTDATE, DateTime.Now.ToUniversalTime().ToString("o"));
            action.Add(NvpCreateRecurringPaymentsProfile.Request._AMT, amount.ToString("f"));
            action.Add(NvpCreateRecurringPaymentsProfile.Request._BILLINGFREQUENCY, "1");
            action.Add(NvpCreateRecurringPaymentsProfile.Request._BILLINGPERIOD, NvpBillingPeriodType.Day);

            action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALBILLINGPERIOD, NvpBillingPeriodType.Day);
            action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALBILLINGFREQUENCY, "1");
            action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALTOTALBILLINGCYCLES, "1");
            action.Add(NvpCreateRecurringPaymentsProfile.Request._TRIALAMT, Double.Parse("0").ToString("f"));

            action.Add(NvpCreateRecurringPaymentsProfile.Request.MAXFAILEDPAYMENTS, "2");
            action.Add(NvpCreateRecurringPaymentsProfile.Request.AUTOBILLOUTAMT, NvpAutoBillType.AddToNextBilling);

            Boolean result = action.Post();
            if (!result)
                throw PaypalException.GenerateException(action);

            this.LblStatus.Text = "Profile ID: " + action.Get(NvpCreateRecurringPaymentsProfile.Response.PROFILEID) + ", Status:" + action.Get(NvpCreateRecurringPaymentsProfile.Response.PROFILESTATUS);
        }

        private String SetExpressCheckout(String email, String subscriptionId)
        {
            NvpSetExpressCheckout checkout = new NvpSetExpressCheckout();
            SetDefaults(checkout);

            checkout.Add(NvpSetExpressCheckout.Request._RETURNURL, Request.Url.ToString());
            checkout.Add(NvpSetExpressCheckout.Request._CANCELURL, GooeyConfigManager.PaypalCancelUrl);
            checkout.Add(NvpSetExpressCheckout.Request.NOSHIPPING, "1");
            if (!String.IsNullOrWhiteSpace(email))
                checkout.Add(NvpSetExpressCheckout.Request.EMAIL, email);
            checkout.Add(NvpSetExpressCheckout.Request.CUSTOM, subscriptionId);
            checkout.Add(new List<NvpBaItem>() { GetBillingAgreement(subscriptionId, "This is a test subscription") });
            checkout.SkipReview = true;

            bool result = checkout.Post();
            if (!result)
                throw PaypalException.GenerateException(checkout);

            //Return the redirect url including the token
            return checkout.RedirectUrl;
        }

        private static NvpBaItem GetBillingAgreement(String subscription, String description)
        {
            NvpBaItem agreement = new NvpBaItem();
            agreement.BillingType = NvpBillingCodeType.RecurringPayments;
            agreement.Custom = subscription;
            agreement.Description = description;
            agreement.PaymentType = NvpMerchantPullPaymentCodeType.Any;

            return agreement;
        }

        private static void SetDefaults(NvpApiBase request)
        {
            NvpConfig.Settings.Version = "54.0";

            request.Credentials.Delegate = GetCredentials;
            request.Environment = (GooeyConfigManager.IsPaypalSandbox) ? NvpEnvironment.Sandbox : NvpEnvironment.Live;
            request.Add(NvpCommonRequest.VERSION, GooeyConfigManager.PaypalApiVersion);
        }

        private static NvpCredentials GetCredentials()
        {
            NvpCredentials creds = new NvpCredentials();
            creds.Username = GooeyConfigManager.PaypalUsername;
            creds.Password = GooeyConfigManager.PaypalPassword;
            creds.Signature = GooeyConfigManager.PaypalSignature;

            return creds;
        }
    }
}