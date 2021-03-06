﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;
using AjaxControlToolkit;
using Gooeycms.Business.Util;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.Themes
{
    public partial class Javascript : ValidatedHelpPage
    {

        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!CurrentSite.Restrictions.IsJavascriptAllowed)
                Response.Redirect("/auth/Manager.aspx?msg=Upgrade+required&feature=js");

            Master.SetTitle("Manage Javascript");
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

            IList<JavascriptFile> files = JavascriptManager.Instance.List(this.GetSelectedTheme());
            IList<JavascriptFile> enabledFiles = new List<JavascriptFile>();
            foreach (JavascriptFile file in files)
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

        }

        protected void LstEnabledFiles_ItemCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            CmsTheme theme = GetSelectedTheme();
            switch (e.CommandName)
            { 
                case "Disable":
                    JavascriptManager.Instance.Disable(theme, e.CommandArgument.ToString());
                    break;
            }

            LoadTabData();
        }

        protected void LstEnabledFiles_Reorder(object sender, ReorderListItemReorderEventArgs e)
        {
            CmsTheme theme = GetSelectedTheme();

            //Get the original order of the items
            IList<JavascriptFile> files = JavascriptManager.Instance.List(this.GetSelectedTheme());
            IList<JavascriptFile> enabledFiles = new List<JavascriptFile>();
            foreach (JavascriptFile file in files)
            {
                if (file.IsEnabled)
                    enabledFiles.Add(file);
            }

            //Reorder the item
            JavascriptFile movedFile = enabledFiles[e.OldIndex];
            enabledFiles.RemoveAt(e.OldIndex);
            enabledFiles.Insert(e.NewIndex, movedFile);

            //Update the ordering of all the items
            int i = 0;
            foreach (JavascriptFile file in enabledFiles)
            {
                file.SortOrder = i++;
                JavascriptManager.Instance.UpdateSortInfo(theme, file.Name, i++);
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
                    JavascriptManager.Instance.Enable(theme, item.Value);
                }
            }
            LoadTabData();
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            JavascriptFile file = JavascriptManager.Instance.Get(this.GetSelectedTheme(), name);

            addScript.Visible = false;
            manageScripts.Visible = false;
            editScriptContent.Visible = true;

            this.Editor.Text = file.Content;

        }

        protected void BtnSaveEdit_Click(object sender, EventArgs e)
        {
            String filename = this.LstExistingFile.SelectedValue;
            byte[] data = Encoding.UTF8.GetBytes(this.Editor.Text);

            CmsTheme theme = GetSelectedTheme();
            JavascriptManager.Instance.Save(theme, filename, data);

            addScript.Visible = true;
            manageScripts.Visible = true;
            editScriptContent.Visible = false;


            LoadTabData();
        }

        protected void BtnDelete_Click(Object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            JavascriptManager.Instance.Delete(this.GetSelectedTheme(), name);
            CurrentSite.Cache.Clear();

            LoadTabData();
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
                data = Encoding.UTF8.GetBytes("//Replace with your javascript content");
            }

            CmsTheme theme = GetSelectedTheme();
            JavascriptManager.Instance.Save(theme, filename, data);

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