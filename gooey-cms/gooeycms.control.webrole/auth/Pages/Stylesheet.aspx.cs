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

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Stylesheet : System.Web.UI.Page
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

            IList<CssFile> files = CssManager.Instance.List(this.GetSelectedPage());
            foreach (CssFile file in files)
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
                    CssManager.Instance.Enable(page, item.Value);
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
                    CssManager.Instance.Disable(page, item.Value);
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

            SelectedPanel = "managepanel";
            LoadTabData();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            String text = "/* Replace with your stylesheet content */";
            String filename = this.TxtFileName.Text;
            byte[] data = Encoding.UTF8.GetBytes(text);

            CmsPage page = GetSelectedPage();
            CssManager.Instance.Save(page, filename, data);

            SelectedPanel = "managepanel";
            LoadTabData();

            this.LstExisting.SelectedValue = filename;
            this.Editor.Text = text;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            String name = this.LstExisting.SelectedValue;
            CssFile file = CssManager.Instance.Get(this.GetSelectedPage(), name);

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