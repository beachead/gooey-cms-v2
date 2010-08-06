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

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Javascript : System.Web.UI.Page
    {
        protected String SelectedPanel = "enablepanel";

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
            this.LstExisting.Items.Clear();
            this.LstEnabledFiles.Items.Clear();
            this.LstDisabledFiles.Items.Clear();

            IList<JavascriptFile> files = JavascriptManager.Instance.List(this.GetSelectedPage());
            foreach (JavascriptFile file in files)
            {
                ListItem item = new ListItem(file.Name, file.FullName);
                this.LstExisting.Items.Add(item);

                if (file.IsEnabled)
                    this.LstEnabledFiles.Items.Add(item);
                else
                    this.LstDisabledFiles.Items.Add(item);
            }
        }

        protected void BtnEnableScripts_Click(object sender, EventArgs e)
        {
            CmsPage page = GetSelectedPage();
            foreach (ListItem item in this.LstDisabledFiles.Items)
            {
                if (item.Selected)
                {
                    JavascriptManager.Instance.Enable(page, item.Value);
                }
            }
            LoadTabData();
        }

        protected void BtnDisableScripts_Click(object sender, EventArgs e)
        {
            CmsPage page = GetSelectedPage();
            foreach (ListItem item in this.LstEnabledFiles.Items)
            {
                if (item.Selected)
                {
                    JavascriptManager.Instance.Disable(page, item.Value);
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

            SelectedPanel = "managepanel";
            LoadTabData();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            String text = "//Replace with your javascript content";
            String filename = this.TxtFileName.Text;
            byte [] data = Encoding.UTF8.GetBytes(text);

            CmsPage page = GetSelectedPage();
            JavascriptManager.Instance.Save(page, filename, data);

            SelectedPanel = "managepanel";
            LoadTabData();

            this.LstExisting.SelectedValue = filename;
            this.Editor.Text = text;
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            String name = this.LstExisting.SelectedValue;
            JavascriptFile file = JavascriptManager.Instance.Get(this.GetSelectedPage(), name);

            this.Editor.Text = file.Content;
            SelectedPanel = "managepanel";
        }

        private CmsPage GetSelectedPage()
        {
            String path = Request.QueryString["pid"];
            CmsPage page = PageManager.Instance.GetLatestPage(path, false);

            return page;
        }
    }
}