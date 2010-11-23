using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;
using Gooeycms.Constants;

namespace Gooeycms.Webrole.Control
{
    public partial class SecureGlobalAdmin : System.Web.UI.MasterPage
    {
        protected static String Root = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            this.LoggedInUsername.Text = LoggedInUser.Wrapper.UserInfo.Firstname;
        }
        protected void OnLogout_Click(Object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
        }

        public void SetTitle(String title)
        {
            Page.Header.Title = String.Format(Page.Header.Title, title);
        }

        protected void SetActive(object sender, EventArgs e)
        {
            HyperLink A = (HyperLink)sender;
            String Url = Request.Url.ToString();
            if (Url.Contains("/" + A.ID.Replace("Nav","").ToLower() + "/"))
            {
                A.Attributes.Add("class", "active");
            }
        }
    }
}
