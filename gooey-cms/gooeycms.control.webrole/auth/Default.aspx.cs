using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Webrole.Control;
using Gooeycms.Webrole.Control.App_Code;

namespace Gooeycms.Webrole.Control.Auth
{
    public partial class Default : ValidatedPage
    {
        protected override void OnLoad(object sender, EventArgs e)
        {
            Master.SetNavigationOn(Secure.NavigationType.Site);
        }
    }
}
