using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Control
{
    public partial class SecureNoNavigation : System.Web.UI.MasterPage
    {
        protected static String Root = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Root == null)
                Root = Page.ResolveUrl("~/");

            this.LoggedInUsername.Text = LoggedInUser.Wrapper.UserInfo.Firstname;
        }

        protected void OnLogout_Click(Object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
        }
    }
}