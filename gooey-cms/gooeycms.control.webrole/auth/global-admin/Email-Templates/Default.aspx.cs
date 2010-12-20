using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.global_admin.Email_Templates
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LnkModifyTemplate_Click(Object sender, CommandEventArgs e)
        {
            String template = (String)e.CommandArgument;

            this.TemplateName.Value = template;
            this.TxtTemplate.Text = GooeyConfigManager.GetEmailTemplate(template);

            this.TxtTemplate.Visible = true;
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            String template = this.TemplateName.Value;
            GooeyConfigManager.SetEmailTemplate(template, this.TxtTemplate.Text);
        }
    }
}