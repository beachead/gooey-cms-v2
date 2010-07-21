using System;

namespace Gooeycms.Business.Pages
{
    class JavascriptManager
    {
        private Data.Model.Page.CmsPage cmsPage;

        public JavascriptManager(Data.Model.Page.CmsPage cmsPage)
        {
            // TODO: Complete member initialization
            this.cmsPage = cmsPage;
        }

        internal String GetJavascriptIncludes()
        {
            //TODO: Implement include code
            return "";
        }
    }
}
