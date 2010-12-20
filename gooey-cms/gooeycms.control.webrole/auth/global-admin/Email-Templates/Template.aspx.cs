using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gooeycms.Webrole.Control.auth.global_admin.Email_Templates
{
    public partial class Template : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.LblTemplateName.Text = Request.QueryString["type"];
            }
        }


    }
}