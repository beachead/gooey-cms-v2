using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;
using gooeycms.constants;
using Gooeycms.Business;

namespace gooeycms.webrole.website
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SalesForceCost.Text = GooeyConfigManager.SalesForcePrice.ToString();
                this.DefaultCmsDomain.Text = GooeyConfigManager.DefaultCmsDomain;
                LoadSelectedPlan(Request.QueryString.ToString().Contains("free"));
            }
        }

        private void LoadSelectedPlan(bool isFree)
        {
            this.SelectedPlan.Items.Clear();

            IList<CmsSubscriptionPlan> plans = Subscriptions.GetSubscriptionPlans();
            foreach (CmsSubscriptionPlan plan in plans)
            {
                ListItem item = new ListItem(plan.Name + " - $" + plan.Price, plan.SKU);
                if ((plan.Price == 0) && (isFree))
                    this.SelectedPlan.Items.Add(item);
                else if (plan.Price > 0)
                    this.SelectedPlan.Items.Add(item);
            }

            if (isFree)
                this.OptionsPanel.Visible = false;
            else
                this.OptionsPanel.Visible = true;
        }

        protected void SelectedPlan_Changed(Object Sender, EventArgs e)
        {
            CmsSubscriptionPlan plan = Subscriptions.GetSubscriptionPlan(this.SelectedPlan.SelectedValue);
            if (plan.Price > 0)
                this.OptionsPanel.Visible = true;
            else
                this.OptionsPanel.Visible = false;
        }

        protected void Subdomain_TextChanged(Object sender, EventArgs e)
        {
            bool isAvailable = Subscriptions.IsSubdomainAvailable(this.Subdomain.Text);
            this.IsAvailableImage.ImageUrl = (isAvailable) ? "images/icon_available.png" : "missingimage.jpg";
            this.IsAvailableImage.Visible = true;
        }

        protected void CreateAccount_Click(Object sender, EventArgs e)
        {
            SubscriptionPlans selectedPlan;
            try
            {
                selectedPlan = (SubscriptionPlans)Enum.Parse(typeof(SubscriptionPlans), this.SelectedPlan.SelectedValue, true);
            }
            catch (Exception)
            {
                throw new ApplicationException("Could not find a mapping for the selected subscription plan: " + this.SelectedPlan.SelectedValue + ". The subscription plan SKU must match one of the following: " + String.Join(" ",Enum.GetNames(typeof(SubscriptionPlans))));
            }

            Registration registration = new Registration();
            registration.Created = DateTime.Now;
            registration.Guid = System.Guid.NewGuid().ToString();
            registration.Firstname = this.Firstname.Text;
            registration.Lastname = this.Lastname.Text;
            registration.Email = this.Email.Text;
            registration.Company = this.Company.Text;
            registration.Sitename = this.Subdomain.Text;
            registration.EncryptedPassword = Registrations.Encrypt(this.Password1.Text);
            registration.SubscriptionPlanId = (int)selectedPlan;
            registration.IsSalesforceEnabled = this.SalesForceOption.Checked;

            Registrations.Save(registration);
            Response.Redirect("Signup-Review.aspx?g=" + registration.Guid, true);
        }
    }
}
