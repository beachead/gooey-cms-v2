using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;
using Microsoft.Security.Application;
using Gooeycms.Business.Cache;
using Gooeycms.Data.Model.Page;
using Gooeycms.Extensions;

namespace Gooeycms.Business.Css
{
    public class CssManager : AssetFileManager<CssFile>
    {
        private static CssManager instance = new CssManager();
        private CssManager() { }
        public static CssManager Instance { get { return CssManager.instance; } }

        public CssManager(Data.Model.Page.CmsPage cmsPage) : base(cmsPage)
        {
        }

        public override string AssetExtension
        {
            get { return CssFile.CssExtension; }
        }


        public override string AssetStorageContainerFormat
        {
            get { return SiteHelper.StylesheetContainerKey; }
        }


        public override string CurrentSiteAssetStorageContainer
        {
            get 
            {
                if (base.SubscriptionId.IsEmpty())
                    return CurrentSite.StylesheetStorageContainer;
                else
                    return String.Format(SiteHelper.StylesheetContainerKey, base.SubscriptionId.Value);
            }
        }
          
        public override string CachePrefix
        {
            get { return "css-files-"; }
        }

        public override string GetIncludes(CmsPage page)
        {
            CmsTheme theme = CurrentSite.GetCurrentTheme();
            StringBuilder includes = new StringBuilder();
            IList<CssFile> scripts = List(theme);
            IList<CssFile> pageScripts = List(page);

            foreach (CssFile script in scripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<link rel=\"stylesheet\" href=\"~/gooeycss/themes/" + AntiXss.UrlEncode(theme.ThemeGuid) + script.FullName + "\" />");
            }

            foreach (CssFile script in pageScripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<link rel=\"stylesheet\" href=\"~/gooeycss/local/" + AntiXss.UrlEncode(page.UrlHash) + script.FullName + "\" />");
            }

            return includes.ToString();
        }

        internal static string Resolve(string content, String key)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.ImagesContainerKey);
            if (key != null)
                container = container + "/" + key;

            Regex pattern = new Regex("url\\('?[^http](?:(?:.*?)/([0-9A-Za-z_ \\-]+\\.(?:png|gif|jpg|jpeg)))'?",RegexOptions.IgnoreCase);
            return pattern.Replace(content, "url('" + container + "/$1'");
        }


    }
}
