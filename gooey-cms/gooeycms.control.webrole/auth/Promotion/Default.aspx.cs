using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Util;
using Gooeycms.Business.Content;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Webrole.Control.auth.Promotion
{
    public partial class Default : App_Code.ValidatedHelpPage
    {
        protected String SelectedPanel = "pagepanel";

        protected override void OnPageLoad(object sender, EventArgs e)
        {
            CmsSubscription subscription = Gooeycms.Business.Subscription.SubscriptionManager.GetSubscription(CurrentSite.Guid);
            this.LblSiteStagingName.Text = subscription.StagingDomain;
            this.LblSiteProductionName.Text = subscription.Domain;
 
        }

        protected void OnItemDataBound(Object sender, GridViewRowEventArgs e)
        {
            CheckBox box = (CheckBox)e.Row.Cells[0].FindControl("approve");
            if (box != null)
            {
                box.Attributes["onclick"] = "onMouseClick(document.getElementById('" + box.ClientID + "'))";
                box.Attributes["onmousedown"] = "onInitialDown(document.getElementById('" + box.ClientID + "'))";
                box.Attributes["onmouseover"] = "onMouseEnter(document.getElementById('" + box.ClientID + "'))";
            }
        }

        protected void PromotePages_Click(Object sender, EventArgs e)
        {
            Boolean promoted = false;
            foreach (GridViewRow row in this.PageListing.Rows)
            {
                CheckBox box = (CheckBox)row.Cells[0].FindControl("approve");
                if (box.Checked)
                {
                    HiddenField temp = (HiddenField)row.Cells[0].FindControl("pageId");
                    String guid = temp.Value;

                    PageManager.Instance.Approve(guid, LoggedInUser.Username);
                    promoted = true;
                }
            }

            if (promoted)
                CurrentSite.RefreshPageCache();

            this.LblPageStatus.Text = "Successfully promoted the selected pages";
            this.PageListing.DataBind();

            SelectedPanel = "pagepanel";
        }

        protected void DeletePages_Click(Object sender, EventArgs e)
        {
            Boolean promoted = false;
            foreach (GridViewRow row in this.PageListing.Rows)
            {
                CheckBox box = (CheckBox)row.Cells[0].FindControl("approve");
                if (box.Checked)
                {
                    HiddenField temp = (HiddenField)row.Cells[0].FindControl("pageId");
                    String guid = temp.Value;

                    PageManager.Instance.Delete(guid);
                    promoted = true;
                }
            }

            if (promoted)
                CurrentSite.RefreshPageCache();

            this.LblPageStatus.Text = "Successfully deleted the selected pages";
            this.PageListing.DataBind();

            SelectedPanel = "pagepanel";
        }

        protected void PromoteContent_Click(Object sender, EventArgs e)
        {
            Boolean promoted = false;
            foreach (GridViewRow row in this.ContentListing.Rows)
            {
                CheckBox box = (CheckBox)row.Cells[0].FindControl("approve");
                if (box.Checked)
                {
                    HiddenField temp = (HiddenField)row.Cells[0].FindControl("contentId");
                    String guid = temp.Value;

                    ContentManager.Instance.Approve(guid, LoggedInUser.Username);
                    promoted = true;
                }
            }

            if (promoted)
                CurrentSite.RefreshPageCache();

            this.LblContentStatus.Text = "Successfully promoted the selected content";
            this.ContentListing.DataBind();

            SelectedPanel = "contentpanel";
        }
    }
}