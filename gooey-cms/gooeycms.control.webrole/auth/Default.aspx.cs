using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using gooeycms.webrole.control.App_Code;

namespace gooeycms.webrole.control.auth
{
    public partial class Default : ValidatedPage
    {
        protected override void OnLoad(object sender, EventArgs e)
        {
            Master.SetNavigationOn(Secure.NavigationType.Site);
        }
    }
}
