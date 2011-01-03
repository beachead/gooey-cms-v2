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
using Gooeycms.Business.Web;

namespace Gooeycms.Webrole.Control
{
    public partial class Secure : SecureNoNavigation
    {
        protected new void Page_Load(Object sender, EventArgs e)
        {
            if (!CurrentSite.IsSet)
                Response.Redirect("~/auth/dashboard.aspx");

            if (CurrentSite.IsAvailable)
            {
                if (this.StagingLink != null)
                    this.StagingLink.NavigateUrl = "http://" + CurrentSite.Subscription.StagingDomain;
                    this.ProdLink.NavigateUrl = "http://" + CurrentSite.Subscription.Domain;

                if (CurrentSite.Subscription.IsDisabled)
                {
                    if (!WebRequestContext.CurrentPage().Path.ToLower().Contains("manage.aspx"))
                        Response.Redirect("~/auth/Manage.aspx?g=" + CurrentSite.Subscription.Guid, true);
                }
            }

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

            if ((!CurrentSite.Subscription.IsCampaignEnabled) && (this.NavCampaigns != null))
                {
                    this.ListItemCampaigns.Visible = false;
                    this.NavCampaigns.Visible = false;
                }
            else  /*  Added if else as annoying li spacing issue was showing up in nav bar when campaigns was not displayed.  */

                {
                this.ListItemCampaigns.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                           SecurityConstants.Roles.SITE_ADMINISTRATOR,
                                                                           SecurityConstants.Roles.SITE_CAMPAIGNS);
                }
 
            this.ListItemPromotion.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                   SecurityConstants.Roles.SITE_ADMINISTRATOR,
                                                                   SecurityConstants.Roles.SITE_PROMOTION);

            this.ListItemThemes.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                SecurityConstants.Roles.SITE_ADMINISTRATOR);

            this.ListItemUser.Visible = LoggedInUser.IsInRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR,
                                                                SecurityConstants.Roles.SITE_ADMINISTRATOR);

        }

        public new Telerik.Web.UI.RadScriptManager ScriptManager
        {
            get { return Master.ScriptManager; }
        }

        public void SetTitle(String title)
        {
            Page.Header.Title = String.Format(Page.Header.Title, title);
        }

        protected void SetActive(object sender, EventArgs e)
        {
            HyperLink A = (HyperLink)sender;
            String Url = Request.Url.ToString();
            if (Url.Contains("/" + A.ID.Replace("Nav", "").ToLower() + "/"))
            {
                A.Attributes.Add("class", "active");
            }
        }
    }
}
