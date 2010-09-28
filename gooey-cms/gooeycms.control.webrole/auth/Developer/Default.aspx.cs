using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business.Membership;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Webrole.Control.auth.Developer
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
            UserInfo user = LoggedInUser.Wrapper.UserInfo;
            IList<Package> packages = SitePackageManager.Instance.GetSitePackagesForUser(user);

            SitePackages.DataSource = packages;
            SitePackages.DataBind();
        }

        protected void SitePackages_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddScreenshot"))
            {
                FileUpload upload = (FileUpload)e.Item.FindControl("FileUpload");
                String packageGuid = (String)e.CommandArgument;

                if (upload.HasFile)
                    SitePackageManager.Instance.AddScreenshot(Data.Guid.New(packageGuid), upload.FileName, upload.FileBytes);
            }
            else if (e.CommandName.Equals("DeletePackage"))
            {
                String packageGuid = (String)e.CommandArgument;
                SitePackageManager.Instance.DeletePackage(packageGuid);
            }

            DoDataBind();
        }

        protected void SitePackages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Package package = (Package)item.DataItem;

                Repeater thumbnails = (Repeater)item.FindControl("ThumbnailImages");
                Repeater features = (Repeater)item.FindControl("FeatureList");
                Label approvalStatus = (Label)item.FindControl("LblApprovalStatus");

                IList<String> thumbnailsrc = SitePackageManager.Instance.GetScreenshotUrls(package);

                thumbnails.DataSource = thumbnailsrc;
                thumbnails.DataBind();

                features.DataSource = package.FeatureList;
                features.DataBind();

                approvalStatus.Text = (package.IsApproved) ? "Approved on " + package.Approved : "AWAITING APPROVAL";
            }
        }

    }
}