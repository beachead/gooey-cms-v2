using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using gooeycms.constants;

namespace gooeycms.webrole.ecommerce
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Subdomain_TextChanged(Object sender, EventArgs e)
        {
            bool isAvailable = Subscriptions.IsSubdomainAvailable(this.Subdomain.Text);
            this.LblAvailable.Text = (isAvailable) ? "AVAILABLE" : "ALREADY IN USE";
        }

        protected void CreateAccount_Click(Object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Created = DateTime.Now;
            registration.Guid = System.Guid.NewGuid().ToString();
            //registration.Firstname = this.Firstname.Text;
            registration.Lastname = this.Lastname.Text;
            registration.Email = this.Email.Text;
            registration.Company = this.Company.Text;
            registration.Sitename = this.Subdomain.Text;
            registration.EncryptedPassword = Registrations.Encrypt(this.Password1.Text);
            registration.SubscriptionTypeId = (int)SubscriptionPlans.Business;
            registration.IsSalesforceEnabled = this.SalesForceOption.Checked;

            Registrations.Save(registration);
            Response.Redirect("Subscribe.aspx?g=" + registration.Guid, true);
        }
    }
}
