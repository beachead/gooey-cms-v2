using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;
using Gooeycms.Constants;
using Gooeycms.Data.Model.Subscription;
using System.Web.Security;
using Gooeycms.Business.Subscription;

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
                LoadWebsites();
                SetMenuDisplay();
            }
        }

        private void LoadWebsites()
        {
            MembershipUserWrapper wrapper = LoggedInUser.Wrapper;
            IList<CmsSubscription> subscriptions = SubscriptionManager.GetSubscriptionsByUserId(wrapper.UserInfo.Id);
            foreach (CmsSubscription subscription in subscriptions)
            {
                ListItem item = new ListItem(subscription.DefaultDisplayName,subscription.Guid);
                if (subscription.Guid.Equals(CurrentSite.Guid.Value))
                    item.Attributes["style"] = "font-weight:bold;";

                this.LstWebsites.Items.Add(item);
            }
        }

        protected void LstWebsites_Click(Object sender, BulletedListEventArgs e)
        {
            ListItem item = this.LstWebsites.Items[e.Index];
            SiteHelper.SetActiveSiteCookie(item.Value);
            SiteHelper.Configure(Data.Guid.New(item.Value));

            Response.Redirect("~/auth/Default.aspx");
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
