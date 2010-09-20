using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;

namespace Gooeycms.Webrole.Ecommerce.store
{
    public partial class Default : System.Web.UI.Page
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
            String packageType = null;

            int lastMaxPos = 0;
            String tempPos = this.LastMaxPos.Value;
            if (!String.IsNullOrEmpty(tempPos))
                lastMaxPos = Int32.Parse(tempPos);

            IList<Package> packages = SitePackageManager.Instance.GetPackages(packageType,lastMaxPos);

            SitePackages.DataSource = packages;
            SitePackages.DataBind();
        }

        protected void SitePackages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Package package = (Package)item.DataItem;

                Repeater thumbnails = (Repeater)item.FindControl("ThumbnailImages");
                Repeater features = (Repeater)item.FindControl("FeatureList");
                HyperLink demolink = (HyperLink)item.FindControl("DemoLink");

                IList<String> thumbnailsrc = SitePackageManager.Instance.GetScreenshotUrls(package);

                thumbnails.DataSource = thumbnailsrc;
                thumbnails.DataBind();

                features.DataSource = package.FeatureList;
                features.DataBind();

                demolink.NavigateUrl = "http://demo-guid.gooeycms.net";
            }
        }
    }
}