using System;
using System.Collections.Generic;
using Gooeycms.Business.Images;
using Gooeycms.Business.Storage;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class ImageBrowser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        public void BtnRefreshImages_Click(Object sender, EventArgs e)
        {
            this.GridExistingImages.DataBind();
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

                    this.GridExistingImages.DataBind();
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
        }

        protected void GridExistingImages_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("RowClick"))
            {
                String guid = (String)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Guid"];
                CmsImage image = ImageManager.Instance.GetImageByGuid(CurrentSite.Guid, guid,false);

                this.ImagePreview.ImageUrl = image.CloudUrl;
            }
        }
    }
}