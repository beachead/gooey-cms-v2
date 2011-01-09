using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business.Membership;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Ecommerce.store
{
    public partial class Confirm : System.Web.UI.Page
    {
        protected Double Amount;
        protected String PackageTitle;
        protected String PackageGuid;
        protected String ReturnUrl;
        protected String ReceiptGuid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!LoggedInUser.IsLoggedIn)
                Response.Redirect("./Purchase.aspx?g=" + Request.QueryString["g"], true);

            String userGuid = LoggedInUser.Wrapper.UserInfo.Guid;
            String packageGuid = Request.QueryString["g"];

            //Make sure the user hasn't already purchased this package
            Boolean exists = SitePackageManager.NewInstance.IsPackageValidForUser(userGuid, packageGuid);
            if (exists)
                Response.Redirect("http://" + GooeyConfigManager.AdminSiteHost + "/auth/dashboard.aspx?msg=" + Server.UrlEncode("You have previously purchased the chosen site and may apply it from your dashboard."));

            if (!Page.IsPostBack)
                DoDataBind();
        }

        private void DoDataBind()
        {
            Data.Guid guid = Data.Guid.New(Request.QueryString["g"]);
            IList<Package> results = new List<Package>();
            Package package = SitePackageManager.NewInstance.GetPackage(guid);

            results.Add(package);
            SitePackages.DataSource = results;
            SitePackages.DataBind();

            this.LblPrice.Text = String.Format("{0:c}", package.Price);
            this.LblPurchaser.Text = LoggedInUser.Wrapper.UserInfo.Firstname + " " + LoggedInUser.Wrapper.UserInfo.Lastname;

            //Create a receipt that we'll use to process this purchase
            Receipt receipt = new Receipt();
            receipt.Amount = package.Price;
            receipt.Created = DateTime.Now;
            receipt.Guid = System.Guid.NewGuid().ToString();
            receipt.PackageGuid = package.Guid;
            receipt.UserGuid = LoggedInUser.Wrapper.UserInfo.Guid;
            receipt.IsComplete = false;
            
            ReceiptManager.Instance.Issue(receipt);

            Amount = package.Price;
            PackageTitle = package.Title;
            PackageGuid = package.Guid;
            ReceiptGuid = receipt.Guid;
            ReturnUrl = GooeyConfigManager.StoreSiteHost + "/complete.aspx";

            BtnPaypalPurchase.PostBackUrl = GooeyConfigManager.PaypalPostUrl;
            if (GooeyConfigManager.IsPaypalSandbox)
                BtnPaypalPurchase.OnClientClick = "alert('This purchase is using the paypal sandbox environment. No actual funds will be transferred.')";

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

                IList<SitePackageManager.PackageScreenshot> thumbnailsrc = SitePackageManager.NewInstance.GetScreenshotUrls(package);

                thumbnails.DataSource = thumbnailsrc;
                thumbnails.DataBind();

                features.DataSource = package.FeatureList;
                features.DataBind();

            }
        }
    }
}