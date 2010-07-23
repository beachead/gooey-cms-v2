using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Gooeycms.Business.Help;
using Gooeycms.Data.Model.Help;

namespace Gooeycms.Webrole.Control.auth.global_admin.Help
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadExisting();
            }
        }

        private void LoadExisting()
        {
            this.ExistingHelpPath.Items.Clear();

            IList<Data.Model.Help.HelpPage> pages = HelpManager.Instance.GetAll();
            foreach(HelpPage page in pages) 
            {
                ListItem item = new ListItem(page.Path, page.Id.ToString());
                this.ExistingHelpPath.Items.Add(item);
            }
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(this.ExistingHelpPath.SelectedValue);
            HelpPage page = HelpManager.Instance.Get(id);

            this.TxtPath.Text = page.Path;
            this.TxtContent.Text = page.Text;
            this.ExistingHelpId.Value = page.Id.ToString();
            this.TxtPath.ReadOnly = true;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            Int32.TryParse(this.ExistingHelpId.Value, out id);
            HelpPage page = HelpManager.Instance.Get(id);
            if (page == null)
                page = new HelpPage();
            
            page.Path = this.TxtPath.Text;
            page.Text = this.TxtContent.Text;

            HelpManager.Instance.Add(page);

            this.ExistingHelpId.Value = page.Id.ToString();

            this.LoadExisting();
        }
    }
}