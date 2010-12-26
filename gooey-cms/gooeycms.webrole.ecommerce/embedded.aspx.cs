using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Gooeycms.Business.Store;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Ecommerce
{
    public partial class embedded : System.Web.UI.Page
    {
        private ICacheManager cache = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DoDataBind();
            }
        }

        private void DoDataBind()
        {
            this.cache = CacheFactory.GetCacheManager();

            Data.Guid guid = Data.Guid.New(Request.QueryString["g"]);
            Package package;

            if (!cache.Contains(guid.Value))
            {
                package = SitePackageManager.NewInstance.GetPackage(guid);
                cache.Add(guid.Value, package,CacheItemPriority.Normal,null,new SlidingTime(TimeSpan.FromMinutes(5)));
            }
            package = (Package)cache.GetData(guid.Value);

            IList<Package> results = new List<Package>();
            results.Add(package);
            SitePackages.DataSource = results;
            SitePackages.DataBind();
        }

        protected void SitePackages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (this.cache == null)
                this.cache = CacheFactory.GetCacheManager();

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