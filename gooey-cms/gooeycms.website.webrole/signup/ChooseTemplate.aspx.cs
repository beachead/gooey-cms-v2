using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;

namespace gooeycms.webrole.website.signup
{
    public partial class ChooseTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            Boolean isValid = Registrations.IsGuidValid(guid);
            if (!isValid)
                Response.Redirect("~/signup/Default.aspx?error=Could+not+find+current+registration+form");

            this.AccountGuid.Value = guid;
        }

        protected void BtnContinue_Click(object sender, EventArgs e)
        {
            String guid = this.AccountGuid.Value;
            Registration registration = Registrations.FindExisting(guid, false);
            if (registration == null)
                Response.Redirect("~/signup/Default.aspx?error=Could+not+find+current+registration+form", true);

            registration.TemplateId = 1;
            Registrations.Save(registration);

            Response.Redirect("~/signup/Payment.aspx?g=" + guid, true);
        }
    }
}
