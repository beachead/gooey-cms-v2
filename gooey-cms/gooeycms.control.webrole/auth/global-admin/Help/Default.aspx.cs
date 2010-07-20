using System;
using Gooeycms.Business.Help;

namespace Gooeycms.Webrole.Control.auth.global_admin.Help
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            Data.Model.Help.HelpPage page = new Data.Model.Help.HelpPage();
            page.Path = this.TxtPath.Text;
            page.Text = this.TxtContent.Text;

            HelpManager.Instance.Add(page);
        }
    }
}