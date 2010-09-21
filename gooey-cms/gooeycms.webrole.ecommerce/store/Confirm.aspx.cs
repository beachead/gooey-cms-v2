using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Ecommerce.store
{
    public partial class Confirm : System.Web.UI.Page
    {
        protected Double Amount;
        protected String PackageTitle;
        protected String PackageGuid;
        protected String ReturnUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                DoDataBind();
        }

        private void DoDataBind()
        {
            Data.Guid guid = Data.Guid.New(Request.QueryString["g"]);
            IList<Package> results = new List<Package>();
            Package package = SitePackageManager.Instance.GetPackage(guid);

            results.Add(package);
            SitePackages.DataSource = results;
            SitePackages.DataBind();

            this.LblPrice.Text = String.Format("{0:c}", package.Price);
            this.LblPurchaser.Text = LoggedInUser.Wrapper.UserInfo.Firstname + " " + LoggedInUser.Wrapper.UserInfo.Lastname;

            Amount = package.Price;
            PackageTitle = package.Title;
            PackageGuid = package.Guid;
            ReturnUrl = "http://store.gooeycms.net/store/complete.aspx";

            /*
             * PAYPAL NOTE:
             * Merchant MUST enable auto-return
             */
        }

        protected void SitePackages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Package package = (Package)item.DataItem;

                Repeater thumbnails = (Repeater)item.FindControl("ThumbnailImages");
                Repeater features = (Repeater)item.FindControl("FeatureList");

                IList<String> thumbnailsrc = SitePackageManager.Instance.GetScreenshotUrls(package);

                thumbnails.DataSource = thumbnailsrc;
                thumbnails.DataBind();

                features.DataSource = package.FeatureList;
                features.DataBind();

            }
        }
    }
}