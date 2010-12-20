using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business.Web;
using Gooeycms.Business;
using Gooeycms.Business.Membership;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class DoPackage : System.Web.UI.Page, Gooeycms.Business.Store.SitePackageManager.IPackageStatusNotifier
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.SelectedFilesCount;
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.TransferSpeed;
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.TimeEstimated;
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.TotalProgress;
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressBar;
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressPercent;
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.FilesCount;
                this.ProgressArea.ProgressIndicators &= ~Telerik.Web.UI.Upload.ProgressIndicators.RequestSize;

                this.ProgressArea.Localization.CurrentFileName = "Package Deployment In Progress:<br />";
                this.ProgressArea.Localization.UploadedFiles = "Progress";
            }
        }

        public void AjaxManager_OnRequest(object sender, AjaxRequestEventArgs e)
        {
            SitePackageManager manager = SitePackageManager.NewInstance;
            Package package = manager.GetPackage(e.Argument);
            manager.CreatePackage(package, this);
            manager.DeployDemoPackage(package.Guid, this);

            //Send an email to the admin
            String body = String.Format(
@"A new package was created which requires approval.
Author: {0}
Package Unique Id: {1}
Owner Subscription Id: {2}
Title: {3}
Date: {4}", LoggedInUser.Username, package.Guid, package.OwnerSubscriptionId, package.Title, package.Created);

            EmailClient client = EmailClient.GetDefaultClient();
            client.ToAddress = GooeyConfigManager.EmailAddresses.SiteAdmin;
            client.FromAddress = LoggedInUser.Email;
            client.Send("New Site Package Requiring Approval", body);

            RadProgressContext progress = RadProgressContext.Current;
            progress.CurrentOperationText = "Successfully Deployed Package";
            progress.OperationComplete = true;
        }

        public void OnNotify(string guid, string eventName, int stepCount, int maxSteps)
        {
            RadProgressContext progress = RadProgressContext.Current;

            progress.PrimaryTotal = 1;
            progress.PrimaryValue = 1;
            progress.PrimaryPercent = 100;

            progress.CurrentOperationText = eventName;
            progress.SecondaryValue = stepCount;
            progress.SecondaryTotal = maxSteps;
            progress.SecondaryPercent = Math.Round(((double)stepCount / (double)maxSteps) * 100);
        }
    }
}