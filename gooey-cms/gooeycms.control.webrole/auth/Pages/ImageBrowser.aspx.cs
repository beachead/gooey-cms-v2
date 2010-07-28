using System;
using System.Collections.Generic;
using Gooeycms.Business.Images;
using Gooeycms.Business.Storage;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class ImageBrowser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.LoadExistingImages();
            }
        }

        private void LoadExistingImages()
        {
            IList<StorageFile> images = ImageManager.Instance.GetAllImagePaths();
            this.AvailableImages.DataSource = images;
            this.AvailableImages.DataBind();
        }

        public void BtnUpload_Click(Object sender, EventArgs e)
        {
            String errorMessage = "";
            if (this.FileUpload.HasFile)
            {
                ImageManager.Instance.AddImage(this.FileUpload.FileName, this.FileUpload.PostedFile.ContentType, this.FileUpload.FileBytes);
            }
            else
            {
                errorMessage = "You must specify an image file or zip file to upload.";
            }

            if (!String.IsNullOrEmpty(errorMessage))
                Anthem.Manager.AddScriptForClientSideEval(String.Format("showErrorMessage('{0}');", errorMessage));
        }
    }
}