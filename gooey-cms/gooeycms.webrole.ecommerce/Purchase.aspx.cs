using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Ecommerce.store
{
    public partial class Purchase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //check if the user is logged in... if so, go to the confirmation page
            if (LoggedInUser.IsLoggedIn)
                Response.Redirect("./Confirm.aspx?g=" + Request.QueryString["g"], true);
            
            if (!Page.IsPostBack)
            {
                DoDataBind();
            }
        }

        private void DoDataBind()
        {
            Data.Guid guid = Data.Guid.New(Request.QueryString["g"]);
            IList<Package> results = new List<Package>();
            Package package = SitePackageManager.Instance.GetPackage(guid);

            results.Add(package);
            SitePackages.DataSource = results;
            SitePackages.DataBind();
        }

        protected void LoginControl_LoggedIn(object sender, EventArgs e)
        {
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

        protected void LnkNewUser_Click(Object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];

            HttpCookie purchase = new HttpCookie("site-purchase");
            purchase.Value = TextEncryption.Encode(guid);
            purchase.Expires = DateTime.Now.AddDays(7);

            Response.Cookies.Add(purchase);

            Response.Redirect("~/Signup.aspx");
        }
    }
}