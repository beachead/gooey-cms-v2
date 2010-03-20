using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gooeycms.webrole.control
{
    public partial class Secure : System.Web.UI.MasterPage
    {
        protected String[] NavigationOn = new String[7] { "", "", "", "", "", "", "" };
        protected static String Root = null;
        public enum NavigationType
        {
            Pages,
            Site,
            Content,
            Campaigns,
            Promotion
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Root == null)
                Root = Page.ResolveUrl("~/");
        }

        protected void OnLogout_Click(Object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
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
