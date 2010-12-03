﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Css;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;
using AjaxControlToolkit;

namespace Gooeycms.Webrole.Control.auth.Themes
{
    public partial class Stylesheet : ValidatedHelpPage
    {
        protected String SelectedPanel = "uploadpanel";
        protected String OutsideSelectedPanel = "modifypanel";

        protected override void OnPageLoad(object sender, EventArgs e)
        {
            Master.SetTitle("Manage Stylesheets");
            if (!Page.IsPostBack)
            {
                LoadTabData();
            }
        }

        private void LoadTabData()
        {
            this.Editor.Text = "";
            this.LstExistingFile.Items.Clear();
            this.LstDisabledFiles.Items.Clear();

            IList<CssFile> files = CssManager.Instance.List(this.GetSelectedTheme());
            IList<CssFile> enabledFiles = new List<CssFile>();
            foreach (CssFile file in files)
            {
                ListItem item = new ListItem(file.Name, file.FullName);
                this.LstExistingFile.Items.Add(item);

                if (!file.IsEnabled)
                    this.LstDisabledFiles.Items.Add(item);

                if (file.IsEnabled)
                    enabledFiles.Add(file);
            }

            this.LstEnabledFilesOrderable.DataSource = enabledFiles;
            this.LstEnabledFilesOrderable.DataBind();

            if (files.Count == enabledFiles.Count)
                this.DisablePanel.Visible = false;
            else
                this.DisablePanel.Visible = true;
        }

        protected void LstEnabledFiles_ItemCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            CmsTheme theme = GetSelectedTheme();
            switch (e.CommandName)
            {
                case "Disable":
                    CssManager.Instance.Disable(theme, e.CommandArgument.ToString());
                    break;
            }

            LoadTabData();
        }

        protected void LstEnabledFiles_Reorder(object sender, ReorderListItemReorderEventArgs e)
        {
            CmsTheme theme = GetSelectedTheme();

            //Get the original order of the items
            IList<CssFile> files = CssManager.Instance.List(this.GetSelectedTheme());
            IList<CssFile> enabledFiles = new List<CssFile>();
            foreach (CssFile file in files)
            {
                if (file.IsEnabled)
                    enabledFiles.Add(file);
            }

            //Reorder the item
            CssFile movedFile = enabledFiles[e.OldIndex];
            enabledFiles.RemoveAt(e.OldIndex);
            enabledFiles.Insert(e.NewIndex, movedFile);

            //Update the ordering of all the items
            int i = 0;
            foreach (CssFile file in enabledFiles)
            {
                CssManager.Instance.UpdateSortInfo(theme, file.Name, i++);
            }

            LoadTabData();
        }

        protected void BtnEnableScripts_Click(object sender, EventArgs e)
        {
            CmsTheme theme = GetSelectedTheme();
            foreach (ListItem item in this.LstDisabledFiles.Items)
            {
                if (item.Selected)
                {
                    CssManager.Instance.Enable(theme, item.Value);
                }
            }
            LoadTabData();
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            CssFile file = CssManager.Instance.Get(this.GetSelectedTheme(), name);

            this.Editor.Text = file.Content;

            OutsideSelectedPanel = "mylibrarypanel";
            SelectedPanel = "editpanel";
        }

        protected void BtnSaveEdit_Click(object sender, EventArgs e)
        {
            String filename = this.LstExistingFile.SelectedValue;
            byte[] data = Encoding.UTF8.GetBytes(this.Editor.Text);

            CmsTheme theme = GetSelectedTheme();
            CssManager.Instance.Save(theme, filename, data);

            OutsideSelectedPanel = "mylibrarypanel";
            SelectedPanel = "editpanel";

            LoadTabData();
        }

        protected void BtnDelete_Click(Object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            CssManager.Instance.Delete(this.GetSelectedTheme(), name);

            LoadTabData();
            OutsideSelectedPanel = "mylibrarypanel";
            SelectedPanel = "editpanel";
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            String filename;
            byte[] data;

            if (this.FileUpload.HasFile)
            {
                filename = this.FileUpload.FileName;
                data = this.FileUpload.FileBytes;
            }
            else
            {
                filename = this.TxtNewFileName.Text;
                data = Encoding.UTF8.GetBytes("/*Replace with your css content*/");
            }

            CmsTheme theme = GetSelectedTheme();
            CssManager.Instance.Save(theme, filename, data);

            OutsideSelectedPanel = "mylibrarypanel";
            SelectedPanel = "editpanel";

            LoadTabData();
        }

        private CmsTheme GetSelectedTheme()
        {
            String guid = Request.QueryString["tid"];
            CmsTheme theme = ThemeManager.Instance.GetByGuid(Data.Guid.New(guid));
            if (theme == null)
                throw new ArgumentException("The specified theme id is not valid.");

            return theme;
        }
    }
}