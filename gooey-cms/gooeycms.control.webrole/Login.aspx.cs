using System;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LoginControl_LoggedIn(object sender, EventArgs e)
        {
            String selectedSiteGuid = MembershipUtil.ProcessLogin(this.LoginControl.UserName);

            String returnUrl = Request.QueryString["ReturnUrl"];
            if (returnUrl != null)
            {
                if (returnUrl.Contains("http"))
                    return;
            }

            //Set the first site active
            if (selectedSiteGuid != null)
                SiteHelper.Configure(selectedSiteGuid);
            else
                Response.Redirect("~/auth/dashboard.aspx", true);
        }
    }
}
