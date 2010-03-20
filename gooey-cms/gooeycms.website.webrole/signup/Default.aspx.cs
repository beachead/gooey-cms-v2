using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;

namespace gooeycms.webrole.website.signup
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            if (guid == null)
                Response.Redirect("Default.aspx?g=" + System.Guid.NewGuid().ToString());

            this.AccountGuid.Value = guid;
        }

        protected void BtnContinue_Click(object sender, EventArgs e)
        {
            String guid = this.AccountGuid.Value;
            if (String.IsNullOrEmpty(guid))
                throw new ApplicationException("The registration form is not in a proper state to process this request. Please refresh the page and try again.");

            Registration registration = Registrations.FindExisting(guid, true);
            String encryptedPassword = Registrations.Encrypt(this.Password.Text);

            registration.Email = this.Email.Text;
            registration.EncryptedPassword = encryptedPassword;

            Registrations.Save(registration);
            Response.Redirect("~/signup/accountinfo.aspx?g=" + guid, true);
        }

    }
}
