using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Javascript;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Pages;
using AjaxControlToolkit;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Javascript : System.Web.UI.Page
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

            IList<JavascriptFile> files = JavascriptManager.Instance.List(this.GetSelectedPage());
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
            CmsPage theme = GetSelectedPage();
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
            CmsPage theme = GetSelectedPage();

            //Get the original order of the items
            IList<JavascriptFile> files = JavascriptManager.Instance.List(this.GetSelectedPage());
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
            CmsPage theme = GetSelectedPage();
            foreach (ListItem item in this.LstDisabledFiles.Items)
            {
                if (item.Selected)
                {
                    JavascriptManager.Instance.Enable(theme, item.Value);
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
                JavascriptManager.Instance.Save(page, filename, data);
            }

            LoadTabData();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            String text = "//Replace with your javascript content";
            String filename = this.TxtFileName.Text;
            byte [] data = Encoding.UTF8.GetBytes(text);

            CmsPage page = GetSelectedPage();
            JavascriptManager.Instance.Save(page, filename, data);

            LoadTabData();

            this.LstExistingFile.SelectedValue = filename;
            this.Editor.Text = text;
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            String name = this.LstExistingFile.SelectedValue;
            JavascriptFile file = JavascriptManager.Instance.Get(this.GetSelectedPage(), name);

            this.Editor.Text = file.Content;

            addScript.Visible = false;
            manageScripts.Visible = false;
            editScriptContent.Visible = true;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            String filename = this.LstExistingFile.SelectedValue;
            byte[] data = Encoding.UTF8.GetBytes(this.Editor.Text);

            CmsPage page = GetSelectedPage();
            JavascriptManager.Instance.Save(page, filename, data);

            addScript.Visible = true;
            manageScripts.Visible = true;
            editScriptContent.Visible = false;

            LoadTabData();
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            addScript.Visible = true;
            manageScripts.Visible = true;
            editScriptContent.Visible = false;
        }

        private CmsPage GetSelectedPage()
        {
            String path = Request.QueryString["pid"];
            CmsPage page = PageManager.Instance.GetLatestPage(path, false);

            return page;
        }
    }
}