﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Web.Microsoft;
using Gooeycms.Data.Model.Campaign;
using Gooeycms.Business.Campaigns;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Default : System.Web.UI.Page
    {
        protected String ExistingCampaignGuid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.SetTitle("Campaign Listings");
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