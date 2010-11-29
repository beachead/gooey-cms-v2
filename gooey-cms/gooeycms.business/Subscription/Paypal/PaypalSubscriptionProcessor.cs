using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Gooeycms.Data.Model.Subscription;
using System.Net;
using System.IO;

namespace Gooeycms.Business.Subscription
{
    public class PaypalSubscriptionProcessor : ISubscriptionProcessor
    {
        public enum TransactionTypes
        {
            Signup,
            Renewal,
            Cancel
        }

        protected HttpRequest request = null;

        #region ISubscriptionProcessor Members
        public virtual void ProcessRequest(HttpRequest request)
        {
            this.request = request;
            ValidateRequest();

            TransactionTypes txType = TransactionType;
            Logging.Info("Processing paypal IPN request type:" + txType);

            switch (txType)
            {
                case TransactionTypes.Signup:
                    ProcessSignup();
                    break;
                default:
                    throw new ArgumentException("The transaction type: " + TransactionType + " is not currently supported.");
            }
        }
        public virtual bool IsDebug
        {
            get { return false; }
        }
        #endregion

        public virtual bool ValidateRequest()
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
            byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
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

        public virtual String Guid
        {
            get
            {
                return this.request["custom"];
            }
        }

        public virtual String TransactionId
        {
            get
            {
                return this.request["txn_id"];
            }
        }

        public virtual String SubscriberId
        {
            get
            {
                return this.request["subscr_id"];
            }
        }

        public virtual TransactionTypes TransactionType
        {
            get
            {
                return ParseTransactionType(this.request["txn_type"]);
            }
        }

        protected TransactionTypes ParseTransactionType(String type)
        {
            if (String.IsNullOrEmpty(type))
                throw new ArgumentException("The transaction type string may not be null or empty.");

            TransactionTypes result;
            switch (type.ToLower())
            {
                case "subscr_signup":
                    result = TransactionTypes.Signup;
                    break;
                case "subscr_payment":
                    result = TransactionTypes.Renewal;
                    break;
                case "subscr_cancel":
                    result = TransactionTypes.Cancel;
                    break;
                default:
                    throw new ArgumentException("Could not map the transaction type " + type + " to a valid transaction type.");
            }

            return result;
        }

        protected virtual void ProcessSignup()
        {
        }
    }
}
