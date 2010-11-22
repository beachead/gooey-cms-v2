using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.global_admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSetupStatus();
        }

        private void CheckSetupStatus()
        {
            if (GooeyStatus.IsMembershipConfigured())
            {
                this.MembershipStatusImage.ImageUrl = "~/images/green-ball.gif";
                this.BtnSetupMembership.Enabled = false;
                this.BtnSetupMembership.Text = "Configured";
            }
            else
            {
                this.MembershipStatusImage.ImageUrl = "~/images/red-ball.gif";
            }

            if (GooeyStatus.IsFlashConfigured())
            {
                this.FlashStatusImage.ImageUrl = "~/images/green-ball.gif";
                this.BtnSetupFlash.Enabled = false;
                this.BtnSetupFlash.Text = "Configured";
            }
            else
            {
                this.FlashStatusImage.ImageUrl = "~/images/red-ball.gif";
            }

        }

        protected void BtnSetupMembership_Click(Object sender, EventArgs e)
        {
            MembershipUtil.ConfigureRoles();
            CheckSetupStatus();
        }

        protected void BtnSetupFlash_Click(Object sender, EventArgs e)
        {
            GooeyStatus.SetupFlash();
            CheckSetupStatus();
        }
    }
}