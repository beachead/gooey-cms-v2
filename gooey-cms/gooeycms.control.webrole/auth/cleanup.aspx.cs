using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth
{
    public partial class cleanup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            String directory = Request.QueryString["d"];

            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesDirectoryKey, guid);
            IStorageClient client = StorageHelper.GetStorageClient();
            client.DeleteSnapshots(imageDirectory, directory);
        }
    }
}