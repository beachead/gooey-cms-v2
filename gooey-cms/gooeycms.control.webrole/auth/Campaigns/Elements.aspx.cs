using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Campaigns;
using Gooeycms.Data.Model.Campaign;
using System.Text;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Util;
using Gooeycms.Business.Cache;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Elements : System.Web.UI.Page
    {
        private String campaignGuid;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.campaignGuid = this.Request["id"];
            if (!Page.IsPostBack)
            {
                LnkGenerateLink.NavigateUrl = "#";
                LnkGenerateLink.Attributes["onclick"] = "window.radopen('Links.aspx?id=" + this.campaignGuid + "'); return false;";
                LoadExisting();
                LoadPages();
            }
        }

        private void LoadExisting()
        {
            this.LstExistingElements.Items.Clear();

            IList<CmsCampaignElement> elements = CampaignManager.Instance.Elements.GetElements(this.campaignGuid);
            foreach (CmsCampaignElement element in elements)
            {
                ListItem item = new ListItem(element.Name, element.Guid);
                this.LstExistingElements.Items.Add(item);
            }
        }

        private void LoadPages()
        {
            IList<CmsPage> pages = PageManager.Instance.Filter(PageManager.Filters.AllPages);
            foreach (CmsPage page in pages)
            {
                ListItem item = new ListItem(page.Url, page.Url);
                this.LstSelectedPages.Items.Add(item);
            }
        }

        protected void BtnAddNew_Click(Object sender, EventArgs e)
        {
            this.LblStatus.Text = "";
            this.ElementGuid.Value = String.Empty;
            this.TxtName.Text = String.Empty;
            this.TxtContent.Text = String.Empty;
            this.TxtPriority.Text = String.Empty;
            this.LstPlacement.SelectedValue = "top";

            foreach (ListItem item in this.LstSelectedPages.Items)
                item.Selected = false;

        }

        protected void BtnEdit_Click(Object sender, EventArgs e)
        {
            this.LblStatus.Text = "";
            CmsCampaignElement element = CampaignManager.Instance.Elements.GetElement(this.LstExistingElements.SelectedValue, this.campaignGuid);
            if (element != null)
            {
                this.ElementGuid.Value = element.Guid;
                this.TxtName.Text = element.Name;
                this.TxtContent.Text = element.Content;
                this.TxtPriority.Text = element.Priority.ToString();
                this.LstPlacement.SelectedValue = element.Placement;

                foreach (ListItem item in this.LstSelectedPages.Items)
                {
                    if (element.Pages.Contains(item.Value))
                        item.Selected = true;
                    else
                        item.Selected = false;
                }
            }
        }

        protected void BtnDelete_Click(Object sender, EventArgs e)
        {
            CmsCampaignElement element = CampaignManager.Instance.Elements.GetElement(this.LstExistingElements.SelectedValue, this.campaignGuid);
            CampaignManager.Instance.Elements.Delete(element);
            this.LoadExisting();

            this.BtnAddNew_Click(null, null);
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            CmsCampaign campaign = CampaignManager.Instance.GetCampaign(this.campaignGuid);

            //Attempt to load the element based upon the guid
            CmsCampaignElement element = CampaignManager.Instance.Elements.GetElement(this.ElementGuid.Value,this.campaignGuid);
            if (element == null)
                element = new CmsCampaignElement();

            int priority = 1;
            Int32.TryParse(this.TxtPriority.Text, out priority);

            element.Campaign = campaign;
            element.Name = this.TxtName.Text;
            element.Placement = this.LstPlacement.SelectedValue;
            element.Priority = priority;
            element.Content = this.TxtContent.Text;

            StringBuilder pages = new StringBuilder();
            foreach (ListItem item in this.LstSelectedPages.Items)
            {
                if (item.Selected)
                    pages.AppendFormat("{0}{1}", item.Value, CmsCampaignElement.ElementSeparator);
            }
            element._Pages = pages.ToString();

            CampaignManager.Instance.Elements.Save(element);
            this.ElementGuid.Value = element.Guid;

            this.LoadExisting();
            this.LstExistingElements.SelectedValue = element.Guid;

            CurrentSite.RefreshPageCache();
            this.LblStatus.Text = "Save Successful. Changes may take a minute or two to be visible on your site.";
        }
    }
}