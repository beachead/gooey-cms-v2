using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Gooeycms.Business.Store;
using Gooeycms.Business.Membership;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.Package_Mgmt
{
    public partial class apply : System.Web.UI.Page, Gooeycms.Business.Store.SitePackageManager.IPackageStatusNotifier
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
            String [] temp = e.Argument.Split('|');

            if (temp.Length == 2)
            {
                String siteGuid = temp[0];
                String packageGuid = temp[1];
                SitePackageManager.NewInstance.DeployToSubscription(LoggedInUser.Wrapper.UserInfo.Guid, siteGuid, packageGuid, this);
            }

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
            progress.SecondaryPercent = ((double)stepCount / (double)maxSteps) * 100;
        }
    }
}