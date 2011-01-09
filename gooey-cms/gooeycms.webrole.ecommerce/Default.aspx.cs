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
using Gooeycms.Business.Membership;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Ecommerce.store
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DoDataBind(0,50000);
            }
        }

        protected void LnkFilterPrice_Click(Object sender, CommandEventArgs e)
        {
            String str = (String)e.CommandArgument;
            String [] temp = str.Split(',');

            double min = 0;
            double max = 50000;
            if (temp.Length == 2)
            {
                Double.TryParse(temp[0], out min);
                Double.TryParse(temp[1], out max);
            }

            DoDataBind(min, max);
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "updatejs", "initSitePanels();", true);
        }

        private void DoDataBind(double minPrice, double maxPrice)
        {
            String packageType = null;

            int lastMaxPos = 0;
            String tempPos = this.LastMaxPos.Value;
            if (!String.IsNullOrEmpty(tempPos))
                lastMaxPos = Int32.Parse(tempPos);

            IList<Package> packages = SitePackageManager.NewInstance.GetApprovedPackages(packageType,lastMaxPos,minPrice,maxPrice);

            SitePackages.DataSource = packages;
            SitePackages.DataBind();
        }

        protected void SitePackages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Package package = (Package)item.DataItem;

                //Find the subscription for this package
                CmsSubscription subscription = SubscriptionManager.GetSubscription(package.Guid);
                CmsSubscription owner = SubscriptionManager.GetSubscription(package.OwnerSubscriptionId);

                String demourl = null;
                if (subscription != null)
                {
                    demourl = "http://" + subscription.Subdomain + GooeyConfigManager.DefaultCmsDomain;
                }

                Image logo = (Image)item.FindControl("LogoSrc");
                Repeater thumbnails = (Repeater)item.FindControl("ThumbnailImages");
                Repeater features = (Repeater)item.FindControl("FeatureList");
                HyperLink demolink = (HyperLink)item.FindControl("DemoLink");
                HyperLink adminLink = (HyperLink)item.FindControl("AdminDemoLink");

                MembershipUserWrapper user = MembershipUtil.FindByUserGuid(owner.PrimaryUserGuid);
                if (user != null)
                    logo.ImageUrl = Logos.GetImageSrc(user.UserInfo.Logo);

                IList<SitePackageManager.PackageScreenshot> thumbnailsrc = SitePackageManager.NewInstance.GetScreenshotUrls(package);
                thumbnails.DataSource = thumbnailsrc;
                thumbnails.DataBind();

                features.DataSource = package.FeatureList;
                features.DataBind();

                if (demourl != null)
                {
                    demolink.NavigateUrl = demourl;
                    adminLink.NavigateUrl = "http://" + GooeyConfigManager.AdminSiteHost + "/demo.aspx?g=" + subscription.Guid;
                }
                else
                {
                    demolink.Attributes["onclick"] = "alert('A demo for this site or theme is not currently available.'); return false;";
                }
            }
        }
    }
}