using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;
using System.Text;

namespace gooeycms.webrole.debug.paypal
{
    public partial class PaypalIpnHandlerDebug : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);

            Logging.Database.Write("Paypal IPN", strRequest);
        }
    }
}