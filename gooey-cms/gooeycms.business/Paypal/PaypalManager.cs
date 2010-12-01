using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Paypal
{
    public class PaypalManager
    {
        private static PaypalManager instance = new PaypalManager();
        private PaypalManager() { }
        public static PaypalManager Instance { get { return PaypalManager.instance; } }

        
        /// <summary>
        /// Validates a paypal PDT response.
        /// </summary>
        /// <param name="txid"></param>
        /// <param name="expectedAmount"></param>
        /// <returns></returns>
        public bool ValidatePDTResponse(string txid, double expectedAmount)
        {
            String accountToken = GooeyConfigManager.PaypalPdtToken;
            if (String.IsNullOrEmpty(accountToken))
                throw new ApplicationException("The paypal IPN or PDT account token is not specified. This is a critical error and must be fixed.");

            WebPostRequest form = new WebPostRequest(GooeyConfigManager.PaypalPostUrl);
            form.Add("cmd", "_notify-synch");
            form.Add("tx", txid);
            form.Add("at", accountToken);
            String result = form.GetResponse();

            return (result.Contains("SUCCESS"));
        }

        public PaypalProfileInfo GetProfileInfo(string profileId)
        {
            PaypalProfileInfo info = null;
            if (!String.IsNullOrEmpty(profileId))
            {
                PaypalExpressCheckout paypal = new PaypalExpressCheckout();
                info = paypal.GetProfileInfo(profileId);
            }
            return info;
        }
    }
}
