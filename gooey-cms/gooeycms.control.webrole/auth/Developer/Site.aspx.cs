using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Store;
using Gooeycms.Business.Membership;
using Gooeycms.Data.Model.Subscription;
using System.Web.Security;
using Gooeycms.Business.Subscription;
using Gooeycms.Business;
using Gooeycms.Data.Model.Store;

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class Site : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (LoggedInUser.IsDemoAccount)
                Response.Redirect("~/auth/default.aspx");

            if (!Page.IsPostBack)
            {
                LoadAvailableSites();
                Anthem.Manager.Register(this);
            }
        }

        private void LoadAvailableSites()
        {
            MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(Membership.GetUser().UserName);
            IList<CmsSubscription> subscriptions = SubscriptionManager.GetSubscriptionsByUserId(wrapper.UserInfo.Id);
            foreach (CmsSubscription subscription in subscriptions)
            {
                ListItem item = new ListItem(subscription.DefaultDisplayName, subscription.Guid);
                this.LstAvailableSites.Items.Add(item);
            }

            IList<String> categories = GooeyConfigManager.StorePackageCategories;
            foreach (String category in categories)
            {
                ListItem item = new ListItem(category, category);
                this.LstCategory.Items.Add(item);
            }
        }

        protected void BtnSave_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            System.Guid.Parse(guid);

            Package package = SitePackageManager.NewInstance.GetPackage(guid);
            if (package == null)
                package = new Package();

            Data.Guid siteGuid = Data.Guid.New(this.LstAvailableSites.SelectedValue);
            package.Category = this.LstCategory.SelectedValue;
            package.Created = DateTime.Now;
            package.Approved = DateTime.MaxValue;
            package.Features = this.TxtFeatures.Text;
            package.Guid = guid;
            package.IsApproved = false;
            package.OwnerSubscriptionId = siteGuid.Value;
            package.PackageType = PackageTypes.Site;
            package.Price = Double.Parse(this.TxtPrice.Text);
            package.Title = this.TxtTitle.Text;

            SitePackageManager.NewInstance.Save(package);

            this.SavedPackageGuid.Value = package.Guid;
        }
    }
}