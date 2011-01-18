using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Store;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Billing;

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class Transfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSearch_Click(Object sender, EventArgs e)
        {
            this.LblStatus.Text = "";

            MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(this.TxtEmailAddress.Text);
            if (wrapper.IsValid())
            {
                this.LblEmailAddress.Text = wrapper.UserInfo.Email;
                this.ConfirmationPanel.Visible = true;
            }
            else
            {
                this.LblStatus.Text = "Could not find a user matching email: " + this.TxtEmailAddress.Text;
                this.LblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void BtnConfirm_Click(Object sender, EventArgs e)
        {
            //Place this package into the user's package purchases
            Package package = SitePackageManager.NewInstance.GetPackage(Request.QueryString["id"]);

            MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(this.TxtEmailAddress.Text);
            SitePackageManager.NewInstance.AddToUser(wrapper.UserInfo.Guid, package);

            //Create a receipt that the package was transfered and from whom
            BillingManager.Instance.AddHistory(wrapper.UserInfo.Guid, null, null, BillingManager.Transfer, 0, "Site package was transferred into account");

            this.LblStatus.Text = "Successfully transfered site to " + this.TxtEmailAddress.Text;
            this.LblStatus.ForeColor = System.Drawing.Color.Green;

            this.TxtEmailAddress.Text = "";

            this.ConfirmationPanel.Visible = false;
        }
    }
}