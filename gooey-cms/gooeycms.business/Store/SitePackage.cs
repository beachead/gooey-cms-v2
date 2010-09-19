using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Css;
using Gooeycms.Business.Storage;
using Gooeycms.Data.Model.Content;

namespace Gooeycms.Business.Store
{
    [Serializable]
    public class SitePackageTheme 
    {
        public CmsTheme Theme { get; set; }
        public IList<CmsTemplate> Templates { get; set; }
        public IList<JavascriptFile> Javascript { get; set; }
        public IList<CssFile> Css { get; set; }
        public IList<StorageFile> Images { get; set; }
    }

    [Serializable]
    public class SitePackagePage
    {
        public CmsPage Page { get; set; }
        public IList<JavascriptFile> Javascript { get; set; }
        public IList<CssFile> Css { get; set; }
        public IList<StorageFile> Images { get; set; }
    }

    [Serializable]
    public class SiteContentType
    {
        public CmsContentType ContentType { get; set; }
        public IList<CmsContentTypeField> Fields { get; set; }
    }

    [Serializable]
    public class SitePackage
    {
        public IList<SitePackageTheme> Themes { get; set; }
        public IList<SitePackagePage> Pages { get; set; }
        public IList<SiteContentType> ContentTypes { get; set; }
    }
}
