using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Util;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Metatags : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String url = Request.QueryString["pid"];
                CmsPage page = PageManager.Instance.GetLatestPage(url);
                if (page != null)
                {
                    this.PageTitle.Text = page.Title;
                    this.PageDescription.Text = page.Description;
                    this.PageKeywords.Text = page.Keywords;
                    this.BodyLoadOptions.Text = page.OnBodyLoad;
                }
            }
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
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
            page.DateSaved = DateTime.Now;
            page.IsApproved = false;
            page.Title = this.PageTitle.Text;
            page.Keywords = this.PageKeywords.Text;
            page.Description = this.PageDescription.Text;
            page.OnBodyLoad = this.BodyLoadOptions.Text;

            page.Author = LoggedInUser.Username;
            page.Culture = CurrentSite.Culture;
            page.Content = existingPage.Content;
            page.Template = existingPage.Template;

            PageManager.Validate(page, isNewPage);
            PageManager.PublishToWorker(page, PageTaskMessage.Actions.Save);

            this.Status.Text = "Successfully updated meta tags";
            this.Status.ForeColor = System.Drawing.Color.Green;
        }
    }
}