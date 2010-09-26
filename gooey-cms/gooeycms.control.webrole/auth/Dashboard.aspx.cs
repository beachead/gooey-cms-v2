using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;

namespace Gooeycms.Webrole.Control.auth
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private IList<CmsSubscription> subscriptions = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(Membership.GetUser().UserName);
            subscriptions = SubscriptionManager.GetSubscriptionsByUserId(wrapper.UserInfo.Id);

            if (!Page.IsPostBack)
            {
                foreach (CmsSubscription subscription in subscriptions)
                {
                    ListItem item = new ListItem(subscription.DefaultDisplayName, subscription.Guid);
                    this.AvailableSites.Items.Add(item);
                }
                DoDataBind();
            }
        }

        private void DoDataBind()
        {
            UserInfo user = LoggedInUser.Wrapper.UserInfo;
            IList<Package> packages = SitePackageManager.Instance.GetPurchasedPackages(user);

            SitePackages.DataSource = packages;
            SitePackages.DataBind();

        }

        protected void BtnManageSite_Click(Object sender, EventArgs e)
        {
            SiteHelper.SetActiveSiteCookie(this.AvailableSites.SelectedValue);
            SiteHelper.Configure(Data.Guid.New(this.AvailableSites.SelectedValue));

            Response.Redirect("~/auth/Default.aspx");
        }

        protected void SitePackages_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddScreenshot"))
            {

            }
        }

        protected void SitePackages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Package package = (Package)item.DataItem;

                Repeater thumbnails = (Repeater)item.FindControl("ThumbnailImages");
                Repeater features = (Repeater)item.FindControl("FeatureList");
                Button applyButton = (Button)item.FindControl("BtnApplyPackage");
                DropDownList siteList = (DropDownList)item.FindControl("LstSites");

                IList<String> thumbnailsrc = SitePackageManager.Instance.GetScreenshotUrls(package);

                thumbnails.DataSource = thumbnailsrc;
                thumbnails.DataBind();

                features.DataSource = package.FeatureList;
                features.DataBind();

                foreach (CmsSubscription subscription in this.subscriptions)
                {
                    ListItem listitem = new ListItem(subscription.DefaultDisplayName, subscription.Guid);
                    siteList.Items.Add(listitem);
                }

                if (package.PackageType == PackageTypes.Site)
                    applyButton.OnClientClick = "return confirm('WARNING!\\r\\nApplying this site will overwrite ALL of your existing content. \\r\\n\\r\\nAre you sure you want to continue?');";
            }
        }
    }
}
