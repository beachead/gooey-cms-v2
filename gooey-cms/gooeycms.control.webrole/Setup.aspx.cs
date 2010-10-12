using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Gooeycms.Constants;

namespace Gooeycms.Webrole.Control
{
    public partial class Setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request["key"].Equals("gooeycms135abcde"))
                throw new ApplicationException("The specified key is not valid.");

            String action = Request.QueryString["a"];
            if ("pwd".Equals(action))
            {
                String username = Request.QueryString["user"];
                String pwd = Request.QueryString["pwd"];

                MembershipUser user = Membership.GetUser(username);
                if (user != null)
                {
                    String password = user.ResetPassword();
                    bool result = user.ChangePassword(password, pwd);
                    if (result)
                    {
                        Response.Write("Password successfully changed");
                    }
                    else
                    {
                        Response.Write("Failed to change password");
                    }
                }
                else
                {
                    Response.Write("Failed to find user: " + username);
                }
            }
            else
            {
                //Setup the ASP.NET Membership roles
                if (!Roles.RoleExists(SecurityConstants.GLOBAL_ADMIN))
                    Roles.CreateRole(SecurityConstants.GLOBAL_ADMIN);
                if (!Roles.RoleExists(SecurityConstants.DOMAIN_ADMIN))
                    Roles.CreateRole(SecurityConstants.DOMAIN_ADMIN);

                String username = Request["user"];
                if (!String.IsNullOrEmpty(username))
                {
                    Roles.AddUserToRole(username, SecurityConstants.GLOBAL_ADMIN);
                    Response.Write("Associated: " + username + " to global admin role");
                }
            }
        }
    }
}
