using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Form;
using Gooeycms.Business.Util;
using Gooeycms.Business.Forms;
using Gooeycms.Business.Crypto;

namespace Gooeycms.webrole.sites
{
    public partial class debugpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["t"] != null)
            {
                string token = Request.QueryString["t"];
                if (!TokenManager.IsValid("cadams@prayer-warrior.net", token))
                    throw new ApplicationException("The specified token: " + token + " is not valid/");
            }
        }

        protected void BtnTest_Click(object sender, EventArgs e)
        {
            this.Token.Text = Server.UrlEncode(TokenManager.Issue("cadams@prayer-warrior.net", TimeSpan.FromMinutes(2)));
        }
    }
}