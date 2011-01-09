using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using System.Web.Security;

namespace Gooeycms.Webrole.Control.auth
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnUpdate_Click(Object sender, EventArgs e)
        {
            try
            {
                bool isUpdated = LoggedInUser.Wrapper.MembershipUser.ChangePassword(this.TxtCurrentPassword.Text, this.TxtPassword1.Text);
                if (isUpdated)
                    this.LblStatus.Text = "Successfully updated password";
                else
                    this.LblStatus.Text = "Your password could not be updated";
            }
            catch (Exception ex)
            {
                this.LblStatus.Text = ex.Message;
            }
        }
    }
}