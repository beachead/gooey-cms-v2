﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;
using System.Text;

namespace Gooeycms.Webrole.Control.auth.Themes
{
    public partial class Templates : ValidatedHelpPage
    {
        private CmsTheme theme;
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            this.theme = ThemeManager.Instance.GetByGuid(Data.Guid.New(Request.QueryString["tid"]));
            if (!Page.IsPostBack)
            {
                this.ThemeName.Text = theme.Name;
                LoadAvailableTemplates(theme);
            }
        }

        private void LoadAvailableTemplates(CmsTheme theme)
        {
            this.TemplateType.Items.Clear();

            IList<String> names = TemplateManager.Instance.GetAvailableGlobalTemplateTypeNames(theme);
            foreach (String name in names)
            {
                ListItem item = new ListItem(name, name);
                this.TemplateType.Items.Add(item);
            }

            IList<CmsTemplate> existings = TemplateManager.Instance.GetTemplates(this.theme);
            this.LstExistingTemplates.Items.Clear();
            foreach (CmsTemplate template in existings)
            {
                ListItem item = new ListItem(template.Name, TextEncryption.Encode(template.Id.ToString()));
                this.LstExistingTemplates.Items.Add(item);
            }
        }

        protected void AddCustomType_Click(object sender, EventArgs e)
        {
            if (this.CustomTemplateType.Visible)
            {
                this.TemplateType.Visible = true;
                this.AddCustomType.Visible = true;

                this.UseGlobalType.Visible = false;
                this.CustomTemplateType.Visible = false;
                this.CustomTemplateTypeLabel.Visible = false;
            }
            else
            {
                this.TemplateType.Visible = false;
                this.AddCustomType.Visible = false;

                this.UseGlobalType.Visible = true;
                this.CustomTemplateType.Visible = true;
                this.CustomTemplateTypeLabel.Visible = true;
            }
        }

        protected void BtnAddTemplate_Click(object sender, EventArgs e)
        {
            this.TemplateContent.Text = "";
            this.ExistingTemplateId.Value = "";

            this.ManageTemplatePanel.Visible = true;

            this.ExistingTemplateName.Visible = false;
            this.TemplateType.Visible = true;
            this.AddCustomType.Visible = true;
        }

        protected void BtnEditTemplate_Click(object sender, EventArgs e)
        {
            CmsTemplate template = TemplateManager.Instance.GetTemplate(Data.EncryptedValue.New(this.LstExistingTemplates.SelectedValue));
            this.TemplateContent.Text = template.Content;
            this.ExistingTemplateName.Text = template.Name;
            this.ExistingTemplateId.Value = this.LstExistingTemplates.SelectedValue;

            this.ExistingTemplateName.Visible = true;
            this.TemplateType.Visible = false;
            this.AddCustomType.Visible = false;
            this.CustomTemplateTypeLabel.Visible = false;
            this.UseGlobalType.Visible = false;
            this.CustomTemplateType.Visible = false;

            this.ManageTemplatePanel.Visible = true;
        }

        protected void BtnDeleteTemplate_Click(object sender, EventArgs e)
        {
        }

        protected void OnSave_Click(object sender, EventArgs e)
        {
            CurrentSite.Cache.Clear();

            String id = this.ExistingTemplateId.Value;
            CmsTemplate template = TemplateManager.Instance.GetTemplate(Data.EncryptedValue.New(id));
            if (template == null)
            {
                template = new CmsTemplate();
                template.IsGlobalTemplateType = (this.TemplateType.Visible);
                if (template.IsGlobalTemplateType)
                    template.Name = this.TemplateType.SelectedValue;
                else
                    template.Name = this.CustomTemplateType.Text;
                template.SubscriptionGuid = SiteHelper.GetActiveSiteGuid(true).Value;
                template.Theme = theme;
            }

            template.Content = this.TemplateContent.Text;
            template.LastSaved = UtcDateTime.Now;

            IList<String> missingImages = new List<String>();
            TemplateManager.Instance.Save(template, missingImages);

            StringBuilder builder = new StringBuilder();
            builder.Append("Successfully saved template.");
            if (missingImages.Count > 0)
            {
                builder.Append(" The following images could not be found and may need to be uploaded: ");
                builder.Append(String.Join(",", missingImages));
            }

            this.LblStatus.Text = builder.ToString();
        }
    }
}
