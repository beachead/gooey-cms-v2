using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Themes;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;

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
            this.LstEnabledFiles.Items.Clear();
            this.LstDisabledFiles.Items.Clear();

            IList<JavascriptFile> files = JavascriptManager.Instance.List(this.GetSelectedTheme());
            foreach (JavascriptFile file in files)
            {
                ListItem item = new ListItem(file.Name, file.FullName);
                this.LstExistingFile.Items.Add(item);

                if (file.IsEnabled)
                    this.LstEnabledFiles.Items.Add(item);
                else
                    this.LstDisabledFiles.Items.Add(item);
            }

            this.LstEnabledFilesOrderable.DataSource = files;
            this.LstEnabledFilesOrderable.DataBind();
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
            foreach (ListItem item in this.LstEnabledFiles.Items)
            {
                if (item.Selected)
                {
                    JavascriptManager.Instance.Disable(theme, item.Value);
                }
            }
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