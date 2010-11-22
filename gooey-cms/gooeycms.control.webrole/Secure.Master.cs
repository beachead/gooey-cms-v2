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

            if (!CurrentSite.Subscription.IsCampaignEnabled)
                this.NavCampaigns.Visible = false;

            if (!Page.IsPostBack)
            {
                SetMenuDisplay();
            }
        }

        protected void SetMenuDisplay()
        {
            this.ListItemContent.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                 SecurityConstants.Roles.SITE_ADMINISTRATOR,
                                                                 SecurityConstants.Roles.SITE_CONTENT_EDITOR);

            this.ListItemPages.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                               SecurityConstants.Roles.SITE_ADMINISTRATOR,
                                                               SecurityConstants.Roles.SITE_PAGE_EDITOR);

            this.ListItemCampaigns.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                   SecurityConstants.Roles.SITE_ADMINISTRATOR,
                                                                   SecurityConstants.Roles.SITE_CAMPAIGNS);

            this.ListItemPromotion.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                   SecurityConstants.Roles.SITE_ADMINISTRATOR,
                                                                   SecurityConstants.Roles.SITE_PROMOTION);
  
            this.ListItemThemes.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                SecurityConstants.Roles.SITE_ADMINISTRATOR);

            this.ListItemUser.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                SecurityConstants.Roles.SITE_ADMINISTRATOR);


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
