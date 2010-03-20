using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace WebRole1
{
    public partial class debug_createuser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            String username = this.Username.Text;
            String[] roles = this.Roles.Text.Split(',');

            MembershipUser user = Membership.CreateUser(username, "1Password!", "cadams@prayer-warrior.net");
            foreach (String role in roles)
            {
                System.Web.Security.Roles.AddUserToRole(username, role);
            }
        }
    }
}
