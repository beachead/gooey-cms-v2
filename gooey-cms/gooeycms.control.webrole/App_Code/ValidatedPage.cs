using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gooeycms.webrole.control.App_Code
{
    public abstract class ValidatedPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["selected-site"] == null)
                Response.Redirect(Resolve("~/auth/Dashboard.aspx"), true);
        }

        protected abstract void OnLoad(object sender, EventArgs e);

        protected String Resolve(String path)
        {
            Control resolver = new Control();
            return resolver.ResolveUrl(path);
        }
    }
}