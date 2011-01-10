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
        public String NavSection = String.Empty;
        public String NavItem = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            dashboard_new.DataBind();
            dashboard_purchase.DataBind();

            NavSection = NavSection.ToLower();
            View activeSetion = (View) FindControl(NavSection);
            mvSubnav.SetActiveView(activeSetion);

            HyperLink activeItem = (HyperLink)activeSetion.FindControl(NavSection + "_" + NavItem);

            if (activeItem != null)
            {
                activeItem.Attributes.Add("class", "active");
            }


        }

        protected void AppendQuerystring(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HyperLink A = (HyperLink)sender;
                A.NavigateUrl = A.NavigateUrl + "?" + Request.QueryString.ToString();
            }
        }

        protected void AppendGUID(object sender, EventArgs e)
        {
            HyperLink A = (HyperLink)sender;
            A.NavigateUrl = A.NavigateUrl + "?g=" + System.Guid.NewGuid().ToString();
        }
    }
}