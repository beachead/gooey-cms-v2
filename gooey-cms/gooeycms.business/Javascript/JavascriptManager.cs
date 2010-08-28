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
using System.IO;

namespace Gooeycms.Business.Javascript
{
    public class JavascriptManager
    {
        private Data.Model.Page.CmsPage cmsPage;
        private static JavascriptManager instance = new JavascriptManager();
        private JavascriptManager() { }
        public static JavascriptManager Instance { get { return JavascriptManager.instance; } }

        public JavascriptManager(Data.Model.Page.CmsPage cmsPage)
        {
            this.cmsPage = cmsPage;
        }

        public String GetJavascriptIncludes(CmsPage page)
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

        private IList<JavascriptFile> List(String key)
        {
            CacheInstance cache = CurrentSite.Cache;

            IList<JavascriptFile> results = cache.Get<IList<JavascriptFile>>("javascript-files-" + key);
            if (results == null)
            {
                results = new List<JavascriptFile>();
                String directory = CurrentSite.JavascriptStorageContainer;
                IStorageClient client = StorageHelper.GetStorageClient();

                IList<StorageFile> files = client.List(directory, key);
                foreach (StorageFile file in files)
                {
                    results.Add(Convert(key, file));
                }
                cache.Add("javascript-files-" + key, results);
            }
            return results;
        }

        public IList<JavascriptFile> List(CmsPage cmsPage)
        {
            return List(cmsPage.UrlHash);
        }

        /// <summary>
        /// Returns a list of the current javascript files
        /// currently associated with this theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<JavascriptFile> List(CmsTheme theme)
        {
            return List(theme.ThemeGuid);
        }


        private void Save(String key, string filename, byte[] data, Boolean enabledByDefault)
        {
            CurrentSite.Cache.Clear("javascript-files-" + key);
            if (!filename.EndsWith(JavascriptFile.Extension))
            {
                filename = filename + JavascriptFile.Extension;
            }

            String enabled = enabledByDefault.StringValue();
            try
            {
                JavascriptFile exists = Get(key, filename);
                enabled = (exists.IsEnabled) ? Boolean.TrueString : Boolean.FalseString;
            }
            catch (PageNotFoundException) { }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), enabled);

            String directory = CurrentSite.JavascriptStorageContainer;
            client.Save(directory, key, filename, data, Permissions.Private);

            if (enabledByDefault)
                SitePageCacheRefreshInvoker.InvokeRefresh(CurrentSite.Guid.Value, SitePageRefreshRequest.PageRefreshType.Staging);
        }

        public void Save(CmsPage page, string filename, byte[] data)
        {
            Save(page.UrlHash, filename, data, true);
        }

        public void Save(CmsTheme theme, string filename, byte[] data)
        {
            Save(theme.ThemeGuid, filename, data, false);
        }

        private void EnableDisable(String key, String filename, Boolean enabled)
        {
            CurrentSite.Cache.Clear("javascript-files-" + key);

            String enabledString = (enabled) ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), enabledString);

            String directory = CurrentSite.JavascriptStorageContainer;
            client.SetMetadata(directory, key, filename);

            SitePageCacheRefreshInvoker.InvokeRefresh(CurrentSite.Guid.Value, SitePageRefreshRequest.PageRefreshType.Staging);
        }

        public void Enable(CmsPage page, string filename)
        {
            EnableDisable(page.UrlHash, filename, true);
        }

        public void Disable(CmsPage page, String filename)
        {
            EnableDisable(page.UrlHash, filename, false);
        }

        public void Enable(CmsTheme theme, string filename)
        {
            EnableDisable(theme.ThemeGuid, filename, true);

        }

        public void Disable(CmsTheme theme, String filename)
        {
            EnableDisable(theme.ThemeGuid, filename, false);
        }

        public JavascriptFile Get(String key, String name)
        {
            String directory = CurrentSite.JavascriptStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetFile(directory, key, name);

            if (!file.Exists())
                throw new PageNotFoundException("The requested resource could not be found. " + name);

            return Convert(key, file);
        }

        public JavascriptFile Get(CmsTheme theme, string name)
        {
            return Get(theme.ThemeGuid, name);
        }

        public JavascriptFile Get(CmsPage page, string name)
        {
            return Get(page.UrlHash, name);
        }


        public void Delete(CmsTheme theme, string name)
        {
            String directory = CurrentSite.JavascriptStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();

            client.Delete(directory, theme.ThemeGuid, name);
            SitePageCacheRefreshInvoker.InvokeRefresh(theme.SubscriptionGuid, SitePageRefreshRequest.PageRefreshType.Staging);
        }

        private static JavascriptFile Convert(String enabledKey, StorageFile file)
        {
            JavascriptFile temp = new JavascriptFile();

            Boolean isEnabled = true;
            String enabled = file.Metadata[GetEnabledKey(enabledKey)];
            Boolean.TryParse(enabled, out isEnabled);

            temp.IsEnabled = isEnabled;
            temp.FullName = file.Filename;

            if ((file.Data != null) && (file.Data.Length > 0))
                temp.Content = Encoding.UTF8.GetString(file.Data);

            return temp;
        }

        private static string GetEnabledKey(String key)
        {
            return "enabled_" + key.Replace("-", "_");
        }
    }
}
