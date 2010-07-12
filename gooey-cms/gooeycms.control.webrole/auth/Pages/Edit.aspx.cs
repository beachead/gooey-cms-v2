using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Webrole.Control.App_Code;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Site;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Site;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Membership;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Edit : HelpPage, IPreviewable
    {
        protected String PageAction = "Add";
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<CmsTemplate> templates = CurrentSite.GetTemplates();
                foreach (CmsTemplate template in templates)
                {
                    ListItem item = new ListItem(template.Name, template.Name);
                    this.PageTemplate.Items.Add(item);
                }

                IList<String> paths = CmsSiteMap.Instance.GetParentPaths();
                foreach (String path in paths)
                {
                    ListItem item = new ListItem(path, path);
                    this.ParentDirectories.Items.Add(item);
                }
            }
        }

        protected void OnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                String existingPageGuid = Request.QueryString["pid"];
                String path = CmsSiteMap.PathCombine(this.ParentDirectories.SelectedValue, this.PageName.Text);

                //If we're adding a new page, make sure the page path isn't already in use
                if (existingPageGuid == null)
                {
                    CmsSitePath sitepath = CmsSiteMap.Instance.GetPath(path);
                    if (sitepath != null)
                        throw new ApplicationException("This page name already exists and may not be used again.");
                }

                CmsPage page = new CmsPage();
                page.DateSaved = DateTime.Now;
                page.IsApproved = false;
                page.Author = LoggedInUser.Username;
                page.Culture = GetSelectedCulture();
                page.Title = this.PageTitle.Text;
                page.Content = this.PageMarkupText.Text;
                page.Keywords = this.PageKeywords.Text;
                page.Description = this.PageDescription.Text;
                page.Template = this.PageTemplate.SelectedValue;
                page.OnBodyLoad = this.BodyLoadOptions.Text;

                CmsPage existingPage = PageManager.Instance.GetLatestPage(path);
                if (existingPage != null)
                {
                    page.Javascript = existingPage.Javascript;
                    page.Stylesheet = existingPage.Stylesheet;
                }

                PageManager.Instance.AddNewPage(this.ParentDirectories.SelectedValue, this.PageName.Text, page);
            }
            catch (Exception ex)
            {
                base.LogException(ex);

                this.Status.Text = ex.Message;
                this.Status.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void OnDelete_Click(Object sender, EventArgs e)
        {
        }

        public string Save()
        {
            //TODO Implement the preview functionality
            /*
            if (Request.QueryString["a"] != null)
            {
                OnSave_Click("preview", null);
                String url = this.ParentDirectories.SelectedItem.Text + "/" + this.PageName.Text;
                return Page.ResolveUrl("~" + url + "?pvw=preview&pvw_id=" + this.savedPageId.ToString());
            }
            else */
                return "::ALERT::You must save the page once before using the preview capability.";
        }

        private String GetSelectedCulture()
        {
            //TODO Actually allow the culture to be selected
            return "en-us";
        }
    }
}
