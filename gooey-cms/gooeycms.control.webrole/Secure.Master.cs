using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control
{
    public partial class Secure : System.Web.UI.MasterPage
    {
        protected static String Root = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentSite.IsSet)
                Response.Redirect("~/auth/dashboard.aspx");

            Page.Header.DataBind();

            this.LoggedInUsername.Text = LoggedInUser.Wrapper.UserInfo.Firstname;
            this.StagingLink.NavigateUrl = "http://" + CurrentSite.StagingDomain;
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
