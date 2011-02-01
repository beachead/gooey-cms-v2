using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Gooeycms.Constants;
using Gooeycms.Business.Membership;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control
{
    public partial class Setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GooeyConfigManager.RemoteAdminIp.Contains(Request.ServerVariables["remote_addr"]))
            {
                String action = Request.QueryString["a"];
                if ("pwd".Equals(action))
                {
                    String username = Request.QueryString["user"];
                    String pwd = Request.QueryString["pwd"];

                    MembershipUser user = Membership.GetUser(username);
                    if (user != null)
                    {
                        user.UnlockUser();
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
                else if ("unlock".Equals(action))
                {
                    String username = Request.QueryString["user"];
                    MembershipUser user = Membership.GetUser(username);
                    if (user != null)
                        user.UnlockUser();

                    Response.Write("Successfully unlocked user");
                }
                else if ("setupdemo".Equals(action))
                {
                    if (!Roles.IsUserInRole(MembershipUtil.DemoAccountUsername, SecurityConstants.Roles.SITE_ADMINISTRATOR))
                        Roles.AddUserToRole(MembershipUtil.DemoAccountUsername, SecurityConstants.Roles.SITE_ADMINISTRATOR);
                }
                else if ("display".Equals(action))
                {
                    String username = Request.QueryString["user"];
                    String[] roles = Roles.GetRolesForUser(username);
                    foreach (String role in roles)
                    {
                        Response.Write(role + "<br />");
                    }
                }
                else if ("addrole".Equals(action))
                {
                    String username = Request.QueryString["user"];
                    String role = Request.QueryString["role"];
                    Roles.AddUserToRole(username, role);

                    Response.Write("Successfully added " + username + " to role " + role);
                }
                else if ("removerole".Equals(action))
                {
                    String username = Request.QueryString["user"];
                    String role = Request.QueryString["role"];
                    if (Roles.IsUserInRole(role))
                        Roles.RemoveUserFromRole(username, role);

                    Response.Write("Successfully removed " + username + " from role " + role);
                }
                else
                {
                    //Setup the ASP.NET Membership roles
                    if (!Roles.RoleExists(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR))
                        Roles.CreateRole(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR);
                    if (!Roles.RoleExists(SecurityConstants.Roles.SITE_ADMINISTRATOR))
                        Roles.CreateRole(SecurityConstants.Roles.SITE_ADMINISTRATOR);
                    if (!Roles.RoleExists(SecurityConstants.Roles.SITE_STANDARD_USER))
                        Roles.CreateRole(SecurityConstants.Roles.SITE_STANDARD_USER);

                    String username = Request["user"];
                    if (!String.IsNullOrEmpty(username))
                    {
                        Roles.AddUserToRole(username, SecurityConstants.Roles.GLOBAL_ADMINISTRATOR);
                        Response.Write("Associated: " + username + " to global admin role");
                    }
                }
            }
            else
            {
                Response.Redirect("/login.aspx");
            }
        }
    }
}
