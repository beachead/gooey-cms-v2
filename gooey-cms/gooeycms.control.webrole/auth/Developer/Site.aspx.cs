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

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class Site : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadAvailableSites();
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
            Data.Guid guid = Data.Guid.New(this.LstAvailableSites.SelectedValue);
            Data.Guid result = SitePackageManager.Instance.CreatePackage(
                                            guid,
                                            this.TxtTitle.Text,
                                            this.TxtFeatures.Text,
                                            this.LstCategory.SelectedValue,
                                            Double.Parse(this.TxtPrice.Text));

            Response.Redirect("./Screenshots.aspx?g=" + result.Value, true);
        }
    }
}