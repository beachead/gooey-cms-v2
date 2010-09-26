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
            MembershipUtil.ProcessLogin(this.LoginControl.UserName);

            String returnUrl = Request.QueryString["ReturnUrl"];
            if (returnUrl != null)
            {
                if (returnUrl.Contains("http"))
                    return;
            }

            Data.Guid guid = SiteHelper.GetActiveSiteGuid();
            if (!guid.IsEmpty())
                SiteHelper.Configure(guid);
            else
                Response.Redirect("~/auth/dashboard.aspx", true);
        }
    }
}
