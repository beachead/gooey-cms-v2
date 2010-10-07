using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Subscription;

namespace gooeycms.webrole.betasignup
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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