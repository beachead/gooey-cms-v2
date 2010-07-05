using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gooeycms.webrole.ecommerce
{
    public partial class Subscribe : System.Web.UI.Page
    {
        public String ReturnUrl;
        public String Guid;
        protected void Page_Load(object sender, EventArgs e)
        {
            ReturnUrl = Page.ResolveUrl(Request.Url + "&success=1");
            Guid = Request.QueryString["g"];
        }
    }
}
