﻿using System;
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
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Storage;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Edit : ValidatedHelpPage, IPreviewable
    {
        private String savedPageId = null;
        private CmsPage existingPage;
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
            String url = Request.QueryString["pid"];

            //always display the latest version
            CmsPage page = PageManager.Instance.GetLatestPage(url);
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

            this.existingPage = page;
        }

        protected void SetPageAction()
        {
            String action = Request.QueryString["a"];
            if ("edit".EqualsCaseInsensitive(action))
                PageAction = "Edit";
        }

        protected void OnSave_Click(Object sender, EventArgs e)
        {
            CmsPage page = new CmsPage();
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

                PageManager.ValidateMarkup(this.PageMarkupText.Text);

                String fullurl = CmsSiteMap.PathCombine(this.ParentDirectories.SelectedValue, this.PageName.Text);
                page.Guid = System.Guid.NewGuid().ToString();
                page.Url = fullurl;
                page.UrlHash = TextHash.MD5(page.Url).Value;
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

                PageManager.PublishToWorker(page,PageTaskMessage.Actions.Save);

                this.Status.Text = "The page has been successfully saved.";
                this.Status.ForeColor = System.Drawing.Color.Green;
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
            if (Request.QueryString["a"] != null)
            {
                PreviewDto dto = new PreviewDto();
                dto.Content = PageMarkupText.Text;
                dto.Title = this.PageTitle.Text;

                QueueManager manager = new QueueManager(QueueManager.GetPreviewQueueName(CurrentSite.Guid));
                manager.ClearQueue();
                manager.Put<PreviewDto>(dto);

                String guid = Request.QueryString["pid"];
                String url = this.ParentDirectories.SelectedItem.Text + this.PageName.Text;
                String token = Server.UrlEncode(TokenManager.Issue(guid, TimeSpan.FromMinutes(5)));
                return Page.ResolveUrl(CurrentSite.Protocol + CurrentSite.StagingDomain + url + "?pvw=preview&pvw_id=" + guid + "&token=" + token);
            }
            else 
                return "::ALERT::You must save the page once before using the preview capability.";
        }

        private String GetSelectedCulture()
        {
            return "en-us";
        }
    }
}
