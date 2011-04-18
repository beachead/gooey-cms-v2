using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Business.Crypto;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Editor : System.Web.UI.Page, IPreviewable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String url = Request.QueryString["pid"];
            if (!Page.IsPostBack)
            {
                IList<CmsTemplate> templates = CurrentSite.GetTemplates();
                foreach (CmsTemplate template in templates)
                {
                    ListItem item = new ListItem(template.Name, template.Name);
                    this.PageTemplate.Items.Add(item);
                }

                CmsPage page = PageManager.Instance.GetLatestPage(url);
                this.PageMarkupText.Text = page.Content;
                this.PageTemplate.SelectedValue = page.Template;
            }
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                String url = Request.QueryString["pid"];

                CmsPage existingPage = PageManager.Instance.GetLatestPage(url);

                CmsPage page = new CmsPage();
                Boolean isNewPage = false;
                String existingPageGuid = Request.QueryString["pid"];
                String path = url;

                String fullurl = url;
                page.Guid = System.Guid.NewGuid().ToString();
                page.Url = fullurl;
                page.UrlHash = TextHash.MD5(page.Url).Value;
                page.SubscriptionId = CurrentSite.Guid.Value;
                page.DateSaved = UtcDateTime.Now;
                page.IsApproved = false;
                page.Author = LoggedInUser.Username;
                page.Culture = CurrentSite.Culture;
                page.Title = existingPage.Title;
                page.Content = this.PageMarkupText.Text;
                page.Keywords = existingPage.Keywords;
                page.Description = existingPage.Description;
                page.Template = this.PageTemplate.SelectedValue;
                page.OnBodyLoad = existingPage.OnBodyLoad;

                PageManager.Validate(page,isNewPage);
                PageManager.PublishToWorker(page, PageTaskMessage.Actions.Save);
                CurrentSite.GetAndSetIsDirty(true);

                this.Status.Text = "The page has been successfully saved.";
                this.Status.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                this.Status.Text = ex.Message;
                this.Status.ForeColor = System.Drawing.Color.Red;
            }
        }

        public string Save()
        {
            String url = Request.QueryString["pid"];
            CmsPage page = PageManager.Instance.GetLatestPage(url);

            PreviewDto dto = new PreviewDto();
            dto.Content = PageMarkupText.Text;
            dto.Title = page.Title;
            dto.TemplateName = this.PageTemplate.SelectedValue;

            QueueManager manager = new QueueManager(QueueManager.GetPreviewQueueName(CurrentSite.Guid));
            manager.ClearQueue();
            manager.Put<PreviewDto>(dto);

            String cacheKey = TextHash.MD5(Request.QueryString["pid"]).Value;

            String token = Server.UrlEncode(TokenManager.Issue(cacheKey, TimeSpan.FromMinutes(5)));
            return Page.ResolveUrl(CurrentSite.Protocol + CurrentSite.StagingDomain + url + "?pvw=preview&pvw_id=" + cacheKey + "&token=" + token);
        }
    }
}