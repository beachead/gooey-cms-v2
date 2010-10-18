using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;
using AjaxControlToolkit;

namespace Gooeycms.Webrole.Control.auth.Themes
{
    public partial class Javascript : ValidatedHelpPage
    {
        protected String SelectedPanel = "uploadpanel";
        protected String OutsideSelectedPanel = "modifypanel";

        protected override void OnPageLoad(object sender, EventArgs e)
        {
            Master.SetNavigationOn(Secure.NavigationType.Site);
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

            //Reorder the item
            JavascriptFile movedFile = files[e.OldIndex];
            files.RemoveAt(e.OldIndex);
            files.Insert(e.NewIndex, movedFile);

            //Update the ordering of all the items
            int i = 0;
            foreach (JavascriptFile file in files)
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

        protected void BtnDisableScripts_Click(object sender, EventArgs e)
        {
            CmsTheme theme = GetSelectedTheme();
            /*
            foreach (ListItem item in this.LstEnabledFiles.Items)
            {
                if (item.Selected)
                {
                    JavascriptManager.Instance.Disable(theme, item.Value);
                }
            }
            */
            LoadTabData();
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            JavascriptFile file = JavascriptManager.Instance.Get(this.GetSelectedTheme(), name);

            this.Editor.Text = file.Content;

            OutsideSelectedPanel = "mylibrarypanel";
            SelectedPanel = "editpanel";
        }

        protected void BtnSaveEdit_Click(object sender, EventArgs e)
        {
            String filename = this.LstExistingFile.SelectedValue;
            byte[] data = Encoding.UTF8.GetBytes(this.Editor.Text);

            CmsTheme theme = GetSelectedTheme();
            JavascriptManager.Instance.Save(theme, filename, data);

            OutsideSelectedPanel = "mylibrarypanel";
            SelectedPanel = "editpanel";

            LoadTabData();
        }

        protected void BtnDelete_Click(Object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            JavascriptManager.Instance.Delete(this.GetSelectedTheme(), name);

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
                data = Encoding.UTF8.GetBytes("//Replace with your javascript content");
            }

            CmsTheme theme = GetSelectedTheme();
            JavascriptManager.Instance.Save(theme, filename, data);

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