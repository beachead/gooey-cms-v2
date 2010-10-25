using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gooeycms.Webrole.Control.Controls
{
    public partial class Subnav : System.Web.UI.UserControl
    {
        public String tid = String.Empty;
        public String navSection = String.Empty;
        public String navItem = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["tid"] != null)
            {
                tid = Request.QueryString["tid"];
            }

            View activeSetion = (View) FindControl(navSection);
            mvSubnav.SetActiveView(activeSetion);

            HyperLink activeItem = (HyperLink)activeSetion.FindControl(navItem);

            if (activeItem != null)
            {
                activeItem.Attributes.Add("class", "active");
            }


        }

        protected void appendTid(object sender, EventArgs e)
        {
            HyperLink A = (HyperLink) sender;
            A.NavigateUrl = A.NavigateUrl + "?tid=" + Request.QueryString["tid"];
        }
    }
}