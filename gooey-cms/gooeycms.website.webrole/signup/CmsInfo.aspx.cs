using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;

namespace gooeycms.webrole.website.signup
{
    public partial class CmsInfo : System.Web.UI.Page
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

            registration.Domain = this.Domain.Text;
            registration.Staging = this.Staging.Text;
            registration.Sitename = this.Sitename.Text;

            Registrations.Save(registration);
            Response.Redirect("~/signup/ChooseTemplate.aspx?g=" + guid, true);
        }
    }
}
