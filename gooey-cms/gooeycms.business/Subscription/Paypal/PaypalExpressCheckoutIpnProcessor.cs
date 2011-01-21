using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Gooeycms.Business.Billing;
using Gooeycms.Data.Model.Subscription;
using System.Net;
using System.IO;

namespace Gooeycms.Business.Subscription.Paypal
{
    public class PaypalExpressCheckoutIpnProcessor : ISubscriptionProcessor
    {
        protected const String RecurringPaymentIdKey = "recurring_payment_id";
        protected const String TransactionIdKey = "txn_type";
        protected const String BilledAmountKey = "amount";
        protected const String TransactionId = "tx_id";

        public class RecurringPaymentTxTypes
        {
            public const String Created = "recurring_payment_profile_created";
            public const String Payment = "recurring_payment";
            public const String Cancelled = "recurring_payment_profile_cancel";
            public const String Suspended = "recurring_payment_suspended_due_to_max_failed_payment";
            public const String Failed = "recurring_payment_failed";
            public const String Expired = "recurring_payment_expired";
        }

        public void ProcessRequest(System.Web.HttpRequest request)
        {
            byte[] param = HttpContext.Current.Request.BinaryRead(request.ContentLength);
            string rawRequest = Encoding.ASCII.GetString(param);

            try
            {
                this.ValidateRequest(request);
            }
            catch (Exception)
            {
                Logging.Database.Write("IPN Notification - Error", "Failed to validate the IPN request: " + rawRequest);
                throw;
            }

            //Determine what type of transaction we are processing
            String txType = request.QueryString[TransactionIdKey];
            String txId = request.QueryString[TransactionId];
            String profileId = request.QueryString[RecurringPaymentIdKey];

            String amountStr = request.QueryString[BilledAmountKey];
            double amount = 0;
            Double.TryParse(amountStr, out amount);

            if (txId == null)
                txId = "";

            //Find the subscription for this profile id
            CmsSubscription subscription = SubscriptionManager.GetSubscriptionByProfileId(profileId);
            if (subscription == null)
            {
                Logging.Database.Write("IPN Notification - Error", "Received an IPN Notification for a subscription which does not exist. Profile ID:" + profileId + "\r\n" + rawRequest);
            }
            else
            {
                switch (txType)
                {
                    case RecurringPaymentTxTypes.Created:
                        //Do nothing for the created txtype, except to log it
                        Logging.Database.Write("IPN Notification", "Paypal recurring profile created: " + profileId + "\r\n" + rawRequest);
                        break;
                    case RecurringPaymentTxTypes.Payment:
                        BillingManager.Instance.AddHistory(subscription.Guid, profileId, txId, txType, amount, "Renewal Processed Successfully");
                        Logging.Database.Write("IPN Notification", "Paypal recurring payment received: " + profileId + "\r\n" + rawRequest);
                        break;
                    case RecurringPaymentTxTypes.Cancelled:
                    case RecurringPaymentTxTypes.Suspended:
                        BillingManager.Instance.AddHistory(subscription.Guid, profileId, txId, txType, amount, "Subscription automatically disabled due to billing cancellation/suspension");
                        Logging.Database.Write("IPN Notification", "Paypal cancellation/suspension received: " + profileId + "\r\n" + rawRequest);
                        this.ProcessCancellation(subscription);
                        break;
                    case RecurringPaymentTxTypes.Expired:
                        BillingManager.Instance.AddHistory(subscription.Guid, profileId, txId, txType, amount, "Subscription free trial has expired.");
                        Logging.Database.Write("IPN Notification", "Paypal billing profile expired received: " + profileId + "\r\n" + rawRequest);
                        break;
                }
            }
        }

        public virtual bool IsDebug
        {
            get { return false; }
        }

        /// <summary>
        /// Detect if the cancellation is for simply the campaign option or for the subscription itself
        /// </summary>
        /// <param name="subscription"></param>
        private void ProcessCancellation(CmsSubscription subscription)
        {
            if (subscription.SubscriptionPlanEnum == Constants.SubscriptionPlans.Free)
            {
                subscription.IsCampaignEnabled = false;
                SubscriptionManager.Save(subscription);
            }
            else
            {
                SubscriptionManager.DisableSubscription(subscription);
            }
        }

        public virtual bool ValidateRequest(System.Web.HttpRequest request)
        {
            //Post back to either sandbox or live
            string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            string strLive = "https://www.paypal.com/cgi-bin/webscr";

            String paypalUrl;
            String test = HttpContext.Current.Request["test_ipn"];
            if (test == "1")
                paypalUrl = strSandbox;
            else
                paypalUrl = strLive;
            Logging.Info("Validating paypal IPN request using url: " + paypalUrl);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = HttpContext.Current.Request.BinaryRead(request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                streamOut.Write(strRequest);
            }

            String strResponse;
            using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                strResponse = streamIn.ReadToEnd();
            }

            Logging.Info("Detected paypal IPN validate response. Response:" + strResponse);
            bool result = false;
            if ("VERIFIED".Equals(strResponse))
                result = true;
            else
                throw new SubscriptionSecurityConstraint("The paypal IPN request is not valid. This request cannot be processed. Result:" + strResponse);

            return result;
        }
    }
}
