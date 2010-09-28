using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Campaigns;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Links : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<CmsPage> pages = PageManager.Instance.Filter(PageManager.Filters.AllPages);
                foreach (CmsPage page in pages)
                {
                    ListItem item = new ListItem(page.Url, page.Url);
                    this.LandingPage.Items.Add(item);
                }

                IList<CmsContent> files = ContentManager.Instance.GetFileContent();
                foreach (CmsContent content in files)
                {
                    String filename = content.FindField("filename").Value;
                    ListItem item = new ListItem(Server.HtmlEncode(filename),Server.HtmlEncode(filename));
                    this.LstFileDownloads.Items.Add(item);
                }
            }
        }

        protected void RdoLinkType_Change(object sender, EventArgs e)
        {
            if (RdoLandingPage.Checked)
            {
                this.PnlLinkTypeLandingPage.Visible = true;
                this.PnlLinkTypeFile.Visible = false;
            }
            else
            {
                this.PnlLinkTypeLandingPage.Visible = false;
                this.PnlLinkTypeFile.Visible = true;
            }
        }

        protected void Generate_Click(object sender, EventArgs e)
        {
            String guid = Request.QueryString["id"];
            String link;
            String landingPage;
            if (RdoFilePage.Checked)
                landingPage = this.LstFileDownloads.SelectedValue;
            else
                landingPage = this.LandingPage.Text;

            link = CampaignManager.Instance.GenerateTrackingLink(guid, this.Source.Text, this.Type.SelectedValue, landingPage, RdoFilePage.Checked);

            CmsUrl url = new CmsUrl(link);

            this.InfoPanel.Visible = false;
            this.ResultPanel.Visible = true;
            this.Result.Text = link;
            this.InternalLink.Text = url.ApplicationRelativePathAndQuery;
        }
    }
}