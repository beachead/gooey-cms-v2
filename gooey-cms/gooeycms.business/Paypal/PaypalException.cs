using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encore.PayPal.Nvp;

namespace Gooeycms.Business.Paypal
{
    public class PaypalException : Exception
    {
        private String errorCode;

        public PaypalException()
        {
        }

        public PaypalException(string errorMessage) : base(errorMessage)
        {
        }

        public static PaypalException GenerateException(Encore.PayPal.Nvp.NvpApiBase details)
        {
            String errorMessage = "";
            if (details.ErrorList.Count > 0)
            {
                NvpError error = details.ErrorList[0];
                errorMessage = error.LongMessage;
            }
            return new PaypalException(errorMessage);
        }
    }
}
