using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.global_admin
{
    public partial class Salesforce : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TxtUsername.Text = GooeyConfigManager.Salesforce.SalesforceUsername;
                this.TxtToken.Text = GooeyConfigManager.Salesforce.SalesforceToken;
            }
        }

        protected void BtnUpdate_Click(Object sender, EventArgs e)
        {
            GooeyConfigManager.Salesforce.SalesforceUsername = this.TxtUsername.Text;
            GooeyConfigManager.Salesforce.SalesforcePassword = this.TxtPassword.Text;
            GooeyConfigManager.Salesforce.SalesforceToken = this.TxtToken.Text;
        }
    }
}