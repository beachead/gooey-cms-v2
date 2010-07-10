using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Page;

namespace Gooeycms.Business.Pages
{
    public class PageManager
    {
        public void Save(CmsPage page)
        {
            IStorageClient storage = new AzureBlobStorageClient();
            storage.Save(CurrentSite.PageStorageKey,page.Guid,"This is a test");
        }
    }
}
