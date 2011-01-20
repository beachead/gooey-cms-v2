using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Page;
using System.Text;
using Gooeycms.Business.Css;
using Gooeycms.Business.Pages;
using AjaxControlToolkit;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Stylesheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

            IList<CssFile> files = CssManager.Instance.List(this.GetSelectedPage());
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
            /*
            if (files.Count == enabledFiles.Count)
                this.DisablePanel.Visible = false;
            else
                this.DisablePanel.Visible = true;
            */
        }

        protected void LstEnabledFiles_ItemCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            CmsPage theme = GetSelectedPage();
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
            CmsPage theme = GetSelectedPage();

            //Get the original order of the items
            IList<CssFile> files = CssManager.Instance.List(this.GetSelectedPage());
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
            CmsPage theme = GetSelectedPage();
            foreach (ListItem item in this.LstDisabledFiles.Items)
            {
                if (item.Selected)
                {
                    CssManager.Instance.Enable(theme, item.Value);
                }
            }
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

                CmsPage page = GetSelectedPage();
                CssManager.Instance.Save(page, filename, data);
            }

            LoadTabData();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            String text = "/* Replace with your stylesheet content */";
            String filename = this.TxtFileName.Text;
            if (!filename.EndsWith(".css"))
                filename = filename + ".css";

            byte[] data = Encoding.UTF8.GetBytes(text);

            CmsPage page = GetSelectedPage();
            CssManager.Instance.Save(page, filename, data);

            LoadTabData();

            this.LstExistingFile.SelectedValue = filename;
            this.Editor.Text = text;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            String filename = this.LstExistingFile.SelectedValue;
            byte[] data = Encoding.UTF8.GetBytes(this.Editor.Text);

            CmsPage page = GetSelectedPage();
            CssManager.Instance.Save(page, filename, data);
            
            addStylesheet.Visible = true;
            manageStylesheets.Visible = true;
            editStylesheetContent.Visible = false;

            LoadTabData();
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            CssFile file = CssManager.Instance.Get(this.GetSelectedPage(), name);

            this.Editor.Text = file.Content;

            addStylesheet.Visible = false;
            manageStylesheets.Visible = false;
            editStylesheetContent.Visible = true;
        }

        protected void BtnDelete_Click(Object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            CssManager.Instance.Delete(this.GetSelectedPage(), name);

            LoadTabData();
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            addStylesheet.Visible = true;
            manageStylesheets.Visible = true;
            editStylesheetContent.Visible = false;
        }

        private CmsPage GetSelectedPage()
        {
            String path = Request.QueryString["pid"];
            CmsPage page = PageManager.Instance.GetLatestPage(path, false);

            return page;
        }
    }
}