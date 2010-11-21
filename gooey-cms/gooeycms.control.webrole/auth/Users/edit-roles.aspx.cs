using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Constants;
using Telerik.Web.UI;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Control.auth.Users
{
    public partial class edit_roles : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MembershipUserWrapper userwrapper = MembershipUtil.FindByUserGuid(Request.QueryString["g"]);
                if (MembershipUtil.IsUserInRole(userwrapper.MembershipUser.UserName, SecurityConstants.Roles.GLOBAL_ADMINISTRATOR))
                    this.LstRoles.Items.Add(RoleListItem(SecurityConstants.Roles.GLOBAL_ADMINISTRATOR, "Allows the user to manage all aspects of gooeycms."));
                
                this.LstRoles.Items.Add(RoleListItem(SecurityConstants.Roles.SITE_ADMINISTRATOR,"Check to allow the user to manage all aspects of the gooeycms site."));
                this.LstRoles.Items.Add(RoleListItem(SecurityConstants.Roles.SITE_PAGE_EDITOR,"Check to allow the user to create/edit/delete cms pages."));
                this.LstRoles.Items.Add(RoleListItem(SecurityConstants.Roles.SITE_CONTENT_EDITOR,"Check to allow the user to create/edit/delete cms content, such as news.")); 
                this.LstRoles.Items.Add(RoleListItem(SecurityConstants.Roles.SITE_CAMPAIGNS,"Check to allow the user to manage site campaigns."));
            }
        }

        private RadListBoxItem RoleListItem(String role, String tooltip)
        {
            MembershipUserWrapper userwrapper = MembershipUtil.FindByUserGuid(Request.QueryString["g"]);
            String value = TextEncryption.Encode(role);
            RadListBoxItem item = new RadListBoxItem(role, value);
            item.ToolTip = tooltip;
            item.Checked = MembershipUtil.IsUserInRole(userwrapper.MembershipUser.UserName, role);
            

            return item;
        }

        protected void LstRoles_Checked(Object sender, RadListBoxItemEventArgs e)
        {
            MembershipUserWrapper userwrapper = MembershipUtil.FindByUserGuid(Request.QueryString["g"]);

            foreach (RadListBoxItem item in ((RadListBox)sender).Items)
            {
                String rolename = TextEncryption.Decode(item.Value);
                if (item.Checked)
                    MembershipUtil.AddUserToRole(userwrapper.MembershipUser.UserName, rolename);
                else
                    MembershipUtil.RemoveUserFromRole(userwrapper.MembershipUser.UserName, rolename);
            }
        }
    }
}