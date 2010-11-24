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
            IList<StorageFile> images = ImageManager.Instance.GetAllImagePaths(StorageClientConst.RootFolder);
            this.AvailableImageList.DataSource = images;
            this.AvailableImageList.DataBind();
        }

        public void BtnUpload_Click(Object sender, EventArgs e)
        {
            String errorMessage = "";
            String files = "";
            if (this.FileUpload.HasFile)
            {
                try
                {
                    IList<StorageFile> results = ImageManager.Instance.AddImage(StorageClientConst.RootFolder, this.FileUpload.FileName, this.FileUpload.PostedFile.ContentType, this.FileUpload.FileBytes);
                    String status = "Successfully uploaded " + results.Count + " images.<br /><br />";
                    files = "Uploaded Files:<br />";
                    foreach (StorageFile file in results)
                    {
                        if (!file.Name.Contains("-thumb"))
                            files = files + Server.HtmlEncode(file.Filename) + "<br />";
                    }

                    LblUploadStatus.Text = status;
                    LblUploadedFiles.Text = files;
                    LoadExistingImages();
                }
                catch (ArgumentException ex)
                {
                    errorMessage = ex.Message;
                }
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