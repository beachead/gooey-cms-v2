using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Gooeycms.webrole.ecommerce
{
    public partial class Cleanup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["all"] != null)
            {
                //cleanup all the users in the asp.net membership system
                foreach (MembershipUser user in System.Web.Security.Membership.GetAllUsers())
                {
                    System.Web.Security.Membership.DeleteUser(user.UserName);
                }
            }
            else
            {
                System.Web.Security.Membership.DeleteUser(Request.QueryString["user"]);
            }
        }
    }   
}
