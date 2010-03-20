using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace gooeycms.webrole.website
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnDeleteUsers_Click(object sender, EventArgs e)
        {
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                Membership.DeleteUser(user.UserName, true);
            }
        }
    }
}
