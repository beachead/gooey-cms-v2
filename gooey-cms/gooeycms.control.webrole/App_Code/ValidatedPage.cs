using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Util;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.App_Code
{
    public abstract class ValidatedPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(SiteHelper.GetActiveSiteGuid()))
                Response.Redirect(Resolve("~/auth/Dashboard.aspx"), true);

            this.OnPageLoad(sender, e);
        }

        protected abstract void OnPageLoad(object sender, EventArgs e);

        protected String Resolve(String path)
        {
            System.Web.UI.Control resolver = new System.Web.UI.Control();
            return resolver.ResolveUrl(path);
        }

        protected void LogException(Exception e)
        {
            if (((e is ApplicationException) ||
                 (e is ArgumentException)))
            {
                //Do nothing
            }
            else
                Logging.Error("There was an unexpected exception", e);
        }
    }
}