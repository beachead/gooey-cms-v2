using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Web.Microsoft;
using Gooeycms.Data.Model.Campaign;
using Gooeycms.Business.Campaigns;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Default : App_Code.ValidatedHelpPage
    {
        protected String ExistingCampaignGuid = "";

        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!CurrentSite.Subscription.IsCampaignEnabled)
                Response.Redirect("~/auth/default.aspx?addon=campaigns", true);

            Master.SetTitle("My Campaigns");
            if (!Page.IsPostBack)
            {
                this.Status.Text = Server.HtmlEncode(Request.QueryString["msg"]);
            }
        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            HiddenField field = GridViewHelper.FindControl<HiddenField>(e.CommandSource, "CampaignId");
            switch (e.CommandName)
            {
                case "deleteid":
                    Delete(field.Value);
                    break;
            }
        }

        private void Delete(String guid)
        {
            CampaignManager.Instance.Delete(guid);
            this.CampaignTable.DataBind();
        }
    }
}