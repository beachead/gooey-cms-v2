using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.global_admin.Developer
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DoDataBind();
            }
        }

        private void DoDataBind()
        {
            IList<Package> packages = SitePackageManager.NewInstance.GetUnapprovedPackages();
            SitePackages.DataSource = packages;
            SitePackages.DataBind();
        }

        protected void SitePackages_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String guid = (String)e.CommandArgument;
            switch (e.CommandName)
            {
                case "ApprovePackage":
                    Approve(guid);
                    break;
            }
            DoDataBind();
        }

        private void Approve(string guid)
        {
            SitePackageManager.NewInstance.ApprovePackage(guid);
        }

        protected void SitePackages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Package package = (Package)item.DataItem;
                CmsSubscription subscription = SubscriptionManager.GetSubscription(package.Guid);
                if (subscription == null)
                    throw new ArgumentException("Could not find the owner's subscription for this package. Subscription ID: " + package.OwnerSubscriptionId);

                Repeater thumbnails = (Repeater)item.FindControl("ThumbnailImages");
                Repeater features = (Repeater)item.FindControl("FeatureList");
                Label approvalStatus = (Label)item.FindControl("LblApprovalStatus");
                Image logo = (Image)item.FindControl("Logo");
                if (logo != null)
                    logo.ImageUrl = Logos.GetImageSrc(subscription.LogoName);
                
                String demourl = "http://" + subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;
                HyperLink demolink = (HyperLink)item.FindControl("DemoLink");
                demolink.NavigateUrl = demourl;

                IList<Gooeycms.Business.Store.SitePackageManager.PackageScreenshot> thumbnailsrc = SitePackageManager.NewInstance.GetScreenshotUrls(package);

                thumbnails.DataSource = thumbnailsrc;
                thumbnails.DataBind();

                features.DataSource = package.FeatureList;
                features.DataBind();
            }
        }
    }
}