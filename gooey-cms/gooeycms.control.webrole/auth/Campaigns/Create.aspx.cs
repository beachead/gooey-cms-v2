using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Campaigns;
using Gooeycms.Data.Model.Campaign;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadExisting();
            }
        }

        private void LoadExisting()
        {
            String guid = Request.QueryString["id"];
            if (!String.IsNullOrWhiteSpace(guid))
            {
                CmsCampaign campaign = CampaignManager.Instance.GetCampaign(guid);
                if (campaign != null)
                {
                    this.Name.Text = campaign.Name;
                    
                    this.Tracking.Text = campaign.TrackingCode;
                    this.Tracking.Enabled = false;

                    this.StartDate.SelectedDate = campaign.StartDate;
                    this.EndDate.SelectedDate = campaign.EndDate;
                }
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String guid = Request.QueryString["id"];
                CmsCampaign campaign = CampaignManager.Instance.GetCampaign(guid);
                if (campaign == null)
                    campaign = new CmsCampaign();

                campaign.Name = this.Name.Text;
                if (campaign.Guid == null)
                    campaign.TrackingCode = this.Tracking.Text;
                
                if (this.StartDate.IsNull)
                    campaign.StartDate = DateTime.Now;
                else
                    campaign.StartDate = this.StartDate.SelectedDate;

                if (this.EndDate.IsNull)
                    campaign.EndDate = DateTime.Now.AddYears(100);
                else
                    campaign.EndDate = this.EndDate.SelectedDate;

                campaign.SubscriptionId = CurrentSite.Guid.Value;

                CampaignManager.Instance.Add(campaign);
                Response.Redirect("./Default.aspx?msg=Successfully+Saved+Campaign");
            }
            catch (Exception ex)
            {
                this.Status.Text = ex.Message;
                this.Status.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}