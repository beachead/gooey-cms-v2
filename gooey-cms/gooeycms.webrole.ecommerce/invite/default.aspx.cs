using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;
using Gooeycms.Business.Subscription;

namespace Gooeycms.Webrole.Ecommerce.invite
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GooeyConfigManager.InviteEmailTemplate == null)
                throw new ApplicationException("The invite template has not been configured. You must configure the email templates before continuing.");
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                InviteManager.Instance.Add(TxtFirstname.Text, TxtLastname.Text, TxtEmail.Text);
                this.LblStatus.Text = "Your invite request has been received.";
            }
            catch (Exception ex)
            {
                this.LblStatus.ForeColor = System.Drawing.Color.Red;
                this.LblStatus.Text = ex.Message;
            }
        }
    }
}