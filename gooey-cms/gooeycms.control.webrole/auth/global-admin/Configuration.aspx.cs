using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.global_admin
{
    public partial class Configuration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && (!Page.IsCallback))
                LoadExistingValue();
        }

        private void LoadExistingValue()
        {
            this.TxtConfigurationValue.Text = GooeyConfigManager.GetValueByReflection(Request.QueryString["d"]);
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            GooeyConfigManager.SetValueAndUpdateCache(Request.QueryString["c"],this.TxtConfigurationValue.Text);
        }
    }
}