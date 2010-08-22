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

namespace Gooeycms.Business.Css
{
    public class CssManager
    {
        private Data.Model.Page.CmsPage cmsPage;
        private static CssManager instance = new CssManager();
        private CssManager() { }
        public static CssManager Instance { get { return CssManager.instance; } }

        public CssManager(Data.Model.Page.CmsPage cmsPage)
        {
            this.cmsPage = cmsPage;
        }

        internal String GetCssIncludes(CmsPage page)
        {
            CmsTheme theme = CurrentSite.GetCurrentTheme();
            StringBuilder includes = new StringBuilder();
            IList<CssFile> scripts = List(theme);
            IList<CssFile> pageScripts = List(page);

            foreach (CssFile script in scripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<link rel=\"stylesheet\" href=\"~/gooeycss/themes/" + AntiXss.UrlEncode(theme.ThemeGuid) + "/" + AntiXss.UrlEncode(script.FullName) + "\" />");
            }

            foreach (CssFile script in pageScripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<link rel=\"stylesheet\" href=\"~/gooeycss/local/" + AntiXss.UrlEncode(page.UrlHash) + "/" + AntiXss.UrlEncode(script.FullName) + "\" />");
            }

            return includes.ToString();
        }

        private IList<CssFile> List(String key)
        {
            CacheInstance cache = CurrentSite.Cache;

            IList<CssFile> results = cache.Get<IList<CssFile>>("css-files-" + key);
            if (results == null)
            {
                results = new List<CssFile>();
                String directory = CurrentSite.StylesheetStorageContainer;
                IStorageClient client = StorageHelper.GetStorageClient();

                IList<StorageFile> files = client.List(directory, key);
                foreach (StorageFile file in files)
                {
                    results.Add(Convert(key, file));
                }
                cache.Add("css-files-" + key, results);
            }
            return results;
        }

        public IList<CssFile> List(CmsPage cmsPage)
        {
            return List(cmsPage.UrlHash);
        }

        /// <summary>
        /// Returns a list of the current css files
        /// currently associated with this theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<CssFile> List(CmsTheme theme)
        {
            return List(theme.ThemeGuid);
        }

        private void Save(String key, string filename, byte[] data, Boolean enabledByDefault)
        {
            CurrentSite.Cache.Clear("css-files-" + key);
            if (!filename.EndsWith(CssFile.Extension))
            {
                filename = filename + CssFile.Extension;
            }

            String enabled = enabledByDefault.StringValue();
            try
            {
                CssFile exists = Get(key, filename);
                enabled = (exists.IsEnabled) ? Boolean.TrueString : Boolean.FalseString;
            }
            catch (PageNotFoundException) { }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), enabled);

            String directory = CurrentSite.StylesheetStorageContainer;
            client.Save(directory, key, filename, data, Permissions.Private);
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
            CurrentSite.Cache.Clear("css-files-" + key);

            String enabledString = (enabled) ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), enabledString);

            String directory = CurrentSite.StylesheetStorageContainer;
            client.SetMetadata(directory, key, filename);
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

        public CssFile Get(String key, String name)
        {
            String directory = CurrentSite.StylesheetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetFile(directory, key, name);

            if (!file.Exists())
                throw new PageNotFoundException("The requested resource could not be found. " + name);

            return Convert(key, file);
        }

        public CssFile Get(CmsTheme theme, string name)
        {
            return Get(theme.ThemeGuid, name);
        }

        public CssFile Get(CmsPage page, string name)
        {
            return Get(page.UrlHash, name);
        }

        public void Delete(CmsTheme theme, string name)
        {
            String directory = CurrentSite.StylesheetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();

            client.Delete(directory, theme.ThemeGuid, name);
        }


        private static CssFile Convert(String enabledKey, StorageFile file)
        {
            CssFile temp = new CssFile();

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

        internal static string Resolve(string content, String key)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.ImagesDirectoryKey);
            if (key != null)
                container = container + "/" + key;

            Regex pattern = new Regex("url\\('?[^http](?:(?:.*?)/([0-9A-Za-z_ \\-]+\\.(?:png|gif|jpg|jpeg)))'?",RegexOptions.IgnoreCase);
            return pattern.Replace(content, "url('" + container + "/$1'");
        }
    }
}
