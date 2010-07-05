using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;

namespace gooeycms.webrole.ecommerce
{
    public partial class PaypalProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Post back to either sandbox or live
            string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            string strLive = "https://www.paypal.com/cgi-bin/webscr";

            String paypalUrl;
            String test = HttpContext.Current.Request["test_ipn"];
            if (test == "1")
            {
                paypalUrl = strSandbox;
            }
            else
            {
                paypalUrl = strLive;
            }

            System.Diagnostics.Trace.TraceInformation("Processing New Paypal IPN Request. Original Request:" + HttpContext.Current.Request.Url.ToString() + ". Using host: " + paypalUrl);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();

            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            System.Diagnostics.Trace.TraceInformation("Paypal IPN Response: " + strResponse);
            if (strResponse == "VERIFIED")
            {
                //Activate the account
                String guid = HttpContext.Current.Request["custom"];
                String txid = HttpContext.Current.Request["txn_id"];
                String txtype = HttpContext.Current.Request["txn_type"];

                System.Diagnostics.Trace.TraceInformation("Paypal IPN Results: " + guid + "," + txid + "," + txtype);
                if ("subscr_signup".Equals(txtype))
                {
                    System.Diagnostics.Trace.TraceInformation("Processing new subscription signup. Guid:" + guid);
                    Registration registration = Registrations.FindExisting(guid, false);
                    if (registration != null)
                    {
                        String subscriberId = HttpContext.Current.Request.Form["subscr_id"];
                        Registrations.ConvertToAccount(registration);
                        System.Diagnostics.Trace.TraceInformation("Successfully created new cms subscription. Paypal Subscriber Id:" + subscriberId);
                    }
                }
                else if ("subscr_payment".Equals(txtype))
                {
                }
                else if ("subscr_cancel".Equals(txtype))
                {
                }
                else
                {
                    System.Diagnostics.Trace.TraceWarning("Unhandled Paypal IPN txtype: " + txtype);
                }
            }
        }

        private void sendErrorEmail(Exception e)
        {
        }
    }
}
