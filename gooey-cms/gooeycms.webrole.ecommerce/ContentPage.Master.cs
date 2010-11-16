using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using System.Web.Security;

namespace Gooeycms.Webrole.Ecommerce
{
    public partial class ContentPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Automatically logout the user if they are logged into the demo account
            if (LoggedInUser.IsDemoAccount)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}
