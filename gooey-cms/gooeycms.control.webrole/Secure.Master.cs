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
        protected String[] NavigationOn = new String[8] { "", "", "", "", "", "", "", "" };
        protected static String Root = null;
        public enum NavigationType
        {
            Pages,
            Site,
            Content,
            Campaigns,
            Promotion,
            Themes
        }
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

        public void SetNavigationOn(NavigationType type)
        {
            switch (type)
            {
                case NavigationType.Content:
                    this.NavigationOn[1] = "on";
                    break;
                case NavigationType.Pages:
                    this.NavigationOn[2] = "on";
                    break;
                case NavigationType.Site:
                    this.NavigationOn[4] = "on";
                    break;
                case NavigationType.Campaigns:
                    this.NavigationOn[3] = "on";
                    break;
                case NavigationType.Promotion:
                    this.NavigationOn[6] = "on";
                    break;
                 case NavigationType.Themes:
                    this.NavigationOn[5] = "on";
                    break;
                default:
                    break;
            }
        }

        public void SetTitle(String title)
        {
            Page.Header.Title = String.Format(Page.Header.Title, title);
        }
    }
}
