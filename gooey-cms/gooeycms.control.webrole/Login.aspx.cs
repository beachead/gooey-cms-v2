using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;

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
        }
    }
}
