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

            Data.Guid guid = SiteHelper.GetActiveSiteGuid();
            SiteHelper.Configure(guid);
        }
    }
}
