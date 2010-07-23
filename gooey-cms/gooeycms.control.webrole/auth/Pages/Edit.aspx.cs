using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Util;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Page;
using Gooeycms.Data.Model.Site;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Edit : ValidatedHelpPage, IPreviewable
    {
        protected String PageAction = "Add";
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            SetPageAction();
            Page.Header.Title = String.Format("{0} Page", PageAction);

            if (!Page.IsPostBack)
            {
                IList<CmsTemplate> templates = CurrentSite.GetTemplates();
                foreach (CmsTemplate template in templates)
                {
                    ListItem item = new ListItem(template.Name, template.Name);
                    this.PageTemplate.Items.Add(item);
                }

                IList<CmsSitePath> paths = CmsSiteMap.Instance.GetParentPaths();
                foreach (CmsSitePath path in paths)
                {
                    ListItem item = new ListItem(path.Url, path.Url);
                    this.ParentDirectories.Items.Add(item);
                }

                if (Request.QueryString["pid"] != null)
                    LoadExisting();
            }
        }

        protected void LoadExisting()
        {
            String guid = Request.QueryString["pid"];
            CmsPage page = PageManager.Instance.GetPage(Data.Guid.New(guid));
            CmsSitePath path = CmsSiteMap.Instance.GetPath(page.Url);

            this.ParentDirectories.SelectedValue = path.Parent;
            this.PageName.Text = path.Name;
            this.PageTitle.Text = page.Title;
            this.PageDescription.Text = page.Description;
            this.PageKeywords.Text = page.Keywords;
            this.PageMarkupText.Text = page.Content;
            this.PageTemplate.SelectedValue = page.Template.ToString();
            this.BodyLoadOptions.Text = page.OnBodyLoad;

            this.ParentDirectories.Enabled = false;
            this.PageName.Enabled = false;

            this.CssManagePanel.Visible = true;
            this.CssNotAvailablePanel.Visible = false;
        }

        protected void SetPageAction()
        {
            String action = Request.QueryString["a"];
            if ("edit".EqualsCaseInsensitive(action))
                PageAction = "Edit";
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
                page.SubscriptionId = CurrentSite.Guid.Value;
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
                
                /*
                 * This is how we detect the difference between a "preview" and a "save"
                 * When the preview button is set, the 'sender' object is a string and this block of code
                 * isn't executed. When a save is called, the 'sender' is an object and this block is executed
                 */
                if (!(sender is String))
                {
                    PageManager.Instance.RemoveObsoletePages(page);
                    Response.Redirect("~/auth/pages/edit.aspx?a=edit&pid=" + page.Guid  + "&s=1", true);
                }
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
