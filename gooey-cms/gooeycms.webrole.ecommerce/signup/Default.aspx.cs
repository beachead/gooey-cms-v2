using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Constants;
using Gooeycms.Business;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Ecommerce
{
    public partial class Signup : System.Web.UI.Page
    {
        protected String TrialExpires = DateTime.Now.AddDays(30).ToString("MMMM dd,yyyy");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HyperLink lnk = (HyperLink)LoginView.FindControl("LnkSignIn");
                if (lnk != null)
                    lnk.NavigateUrl = "http://" + GooeyConfigManager.AdminSiteHost + "/login.aspx?ReturnUrl=" + Server.UrlEncode("http://store.gooeycms.net/signup/");

                this.SalesForceCost.Text = GooeyConfigManager.SalesForcePrice.ToString();
                this.DefaultCmsDomain.Text = GooeyConfigManager.DefaultCmsDomain;
                LoadSelectedPlan(Request.QueryString.ToString().Contains("free"));

                LoadExistingData();
            }
        }

        private void LoadExistingData()
        {
            this.PnlCreatePassword.Visible = true;
            this.PnlNoPassword.Visible = false;

            if (LoggedInUser.IsLoggedIn)
            {
                UserInfo user = LoggedInUser.Wrapper.UserInfo;
                this.Firstname.Text = user.Firstname;
                this.Firstname.Enabled = false;

                this.Lastname.Text = user.Lastname;
                this.Lastname.Enabled = false;

                this.Email.Text = user.Email;
                this.Email.Enabled = false;

                this.Company.Text = user.Company;
                this.Company.Enabled = false;

                this.PnlCreatePassword.Visible = false;
                this.PnlNoPassword.Visible = true;
            }
        }

        private void LoadSelectedPlan(bool isFree)
        {
            this.SelectedPlan.Items.Clear();

            IList<CmsSubscriptionPlan> plans = SubscriptionManager.GetSubscriptionPlans();
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
            CmsSubscriptionPlan plan = SubscriptionManager.GetSubscriptionPlan(this.SelectedPlan.SelectedValue);
            if (plan.Price > 0)
                this.OptionsPanel.Visible = true;
            else
                this.OptionsPanel.Visible = false;
        }

        protected void Subdomain_TextChanged(Object sender, EventArgs e)
        {
            bool isAvailable = SubscriptionManager.IsSubdomainAvailable(this.Subdomain.Text);
            this.IsAvailableImage.ImageUrl = (isAvailable) ? "../images/icon_available.png" : "missingimage.jpg";
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
            if (LoggedInUser.IsLoggedIn)
                registration.ExistingAccountGuid = LoggedInUser.Wrapper.UserInfo.Guid;

            Registrations.Save(registration);
            Response.Redirect("Signup-Review.aspx?g=" + registration.Guid, true);
        }
    }
}
