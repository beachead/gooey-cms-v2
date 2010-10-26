using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;
using Microsoft.Security.Application;
using Gooeycms.Business.Cache;
using Gooeycms.Data.Model.Page;
using Gooeycms.Extensions;
using System.IO;

namespace Gooeycms.Business.Javascript
{
    public class JavascriptManager : AssetFileManager<JavascriptFile>
    {
        private static JavascriptManager instance = new JavascriptManager();
        private JavascriptManager() { }
        public static JavascriptManager Instance { get { return JavascriptManager.instance; } }

        public JavascriptManager(Data.Model.Page.CmsPage cmsPage) : base(cmsPage)
        {
        }

        public override string AssetExtension
        {
            get { return JavascriptFile.JavascriptExtension; }
        }

       
        public override string AssetStorageContainerFormat
        {
            get { return SiteHelper.JavascriptContainerKey; }
        }
        
        
        public override string CurrentSiteAssetStorageContainer
        {
            get { return CurrentSite.JavascriptStorageContainer; }
        }

        public override string CachePrefix
        {
            get { return "javascript-files-"; }
        }

        public override String GetIncludes(CmsPage page)
        {
            CmsTheme theme = CurrentSite.GetCurrentTheme();
            StringBuilder includes = new StringBuilder();
            IList<JavascriptFile> scripts = List(theme);
            IList<JavascriptFile> pageScripts = List(page);

            foreach (JavascriptFile script in scripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<script src=\"~/gooeyscripts/themes/" + AntiXss.UrlEncode(theme.ThemeGuid) + "/" + AntiXss.UrlEncode(script.FullName) + "\" type=\"text/javascript\" language=\"javascript\"></script>");
            }

            foreach (JavascriptFile script in pageScripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<script src=\"~/gooeyscripts/local/" + AntiXss.UrlEncode(page.UrlHash) + "/" + AntiXss.UrlEncode(script.FullName) + "\" type=\"text/javascript\" language=\"javascript\"></script>");
            }

            return includes.ToString();
        }
    }
}
