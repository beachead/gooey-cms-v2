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
using Gooeycms.Data.Model.Site;

namespace Gooeycms.Business.Store
{
    [Serializable]
    public class SiteContent
    {
        public CmsContent Content { get; set; }
        public Boolean IsFileType { get; set; }
        public String Filename { get; set; }
        public byte[] FileData { get; set; }
    }

    [Serializable]
    public class SitePackageTheme 
    {
        public CmsTheme Theme { get; set; }
        public String Header { get; set; }
        public String Footer { get; set; }
        public IList<CmsTemplate> Templates { get; set; }
        public IList<JavascriptFile> Javascript { get; set; }
        public IList<CssFile> Css { get; set; }
        public IList<BlobSnapshot> Images { get; set; }
    }

    [Serializable]
    public class SitePackagePage
    {
        public CmsPage Page { get; set; }
        public IList<JavascriptFile> Javascript { get; set; }
        public IList<CssFile> Css { get; set; }
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
        public Data.Guid OriginalSiteGuid { get; set; }
        public IList<SitePackageTheme> Themes { get; set; }
        public IList<SitePackagePage> Pages { get; set; }
        public IList<SiteContentType> ContentTypes { get; set; }
        public IList<SiteContent> SiteContent { get; set; }
        public IList<CmsSitePath> SiteMapPaths { get; set; }
        public IList<BlobSnapshot> PageImages { get; set; }
    }
}
