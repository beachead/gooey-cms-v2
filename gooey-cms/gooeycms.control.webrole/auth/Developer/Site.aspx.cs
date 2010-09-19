using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Store;

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class Site : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            Data.Guid result = SitePackageManager.Instance.CreatePackage(Data.Guid.New("cc69e774-71d8-4753-b71b-9a23dd98f309"), "test", "test", "test", 300.50);
            Response.Redirect("./Screenshots.aspx?g=" + result.Value, true);
        }
    }
}