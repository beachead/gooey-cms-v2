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
                this.BtnSetupMembership.Enabled = true;
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
                this.BtnSetupFlash.Enabled = true;
            }

            if (GooeyStatus.IsDevelopmentMode())
            {
                this.DevModeStatusImage.ImageUrl = "~/images/yellow-ball.gif";
                this.LblDevMode.Text = "Running outside of Windows Azure (DEVELOPMENT MODE)";
            }
            else
            {
                this.FlashStatusImage.ImageUrl = "~/images/green-ball.gif";
                this.LblDevMode.Text = "Running within Windows Azure (PRODUCTION MODE)";
            }

            if (GooeyStatus.IsPaypalSandbox())
            {
                this.PaypalStatusImage.ImageUrl = "~/images/yellow-ball.gif";
                this.BtnTogglePaypal.Text = "Enable Paypal LIVE mode";
                this.BtnTogglePaypal.OnClientClick = "return confirm('Are you sure you want to ENABLE live paypal payments?');";
            }
            else
            {
                this.PaypalStatusImage.ImageUrl = "~/images/green-ball.gif";
                this.BtnTogglePaypal.Text = "Enable Paypal SANDBOX mode";
                this.BtnTogglePaypal.OnClientClick = "return confirm('Are you sure you want to DISABLE live paypal payments and begin using the sandbox?');";
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

        protected void BtnTogglePaypal_Click(Object sender, EventArgs e)
        {
            GooeyStatus.TogglePaypalMode();
            CheckSetupStatus();
        }
    }
}