using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;

namespace Gooeycms.webrole.sites.gooey_static
{
    public partial class Static : System.Web.UI.MasterPage
    {
        protected String ResourceUrl = GooeyConfigManager.StaticResourceUrl;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}