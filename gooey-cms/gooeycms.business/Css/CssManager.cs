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

        internal String GetCssIncludes()
        {
            StringBuilder includes = new StringBuilder();
            IList<CssFile> scripts = List(CurrentSite.GetCurrentTheme());
            foreach (CssFile script in scripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<link rel=\"stylesheet\" href=\"~/gooeycss/" + AntiXss.UrlEncode(script.FullName) + "\" />");
            }

            return includes.ToString();
        }

        /// <summary>
        /// Returns a list of the current javascript files
        /// currently associated with this theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<CssFile> List(CmsTheme theme)
        {
            CacheInstance cache = CurrentSite.Cache;

            IList<CssFile> results = cache.Get<IList<CssFile>>("css-files");
            if (results == null)
            {
                results = new List<CssFile>();
                
                String container = CurrentSite.StylesheetStorageContainer;
                IStorageClient client = StorageHelper.GetStorageClient();

                IList<StorageFile> files = client.List(container, theme.ThemeGuid);
                foreach (StorageFile file in files)
                {
                    results.Add(Convert(theme, file));
                }
                cache.Add("css-files", results);
            }
            return results;
        }

        /// <summary>
        /// Associates a new file with the specified theme
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        public void Save(CmsTheme theme, string filename, byte[] data)
        {
            CurrentSite.Cache.Clear("css-files");

            if (!filename.EndsWith(CssFile.Extension))
            {
                filename = filename + CssFile.Extension;
            }

            String enabled = Boolean.FalseString;
            try
            {
                CssFile exists = Get(theme, filename);
                enabled = (exists.IsEnabled) ? Boolean.TrueString : Boolean.FalseString;
            }
            catch (PageNotFoundException) { }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), enabled);

            String directory = CurrentSite.StylesheetStorageContainer;
            client.Save(directory, theme.ThemeGuid, filename, data, Permissions.Private);
        }

        /// <summary>
        /// Enables the specified javascript file to run on the theme
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="filename"></param>
        public void Enable(CmsTheme theme, string filename)
        {
            CurrentSite.Cache.Clear("css-files");



            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), "true");

            String directory = CurrentSite.StylesheetStorageContainer;
            client.SetMetadata(directory, theme.ThemeGuid, filename);
        }

        public void Disable(CmsTheme theme, String filename)
        {
            CurrentSite.Cache.Clear("css-files");

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), "false");

            String directory = CurrentSite.StylesheetStorageContainer;
            client.SetMetadata(directory, theme.ThemeGuid, filename);
        }

        public CssFile Get(CmsTheme theme, string name)
        {
            String directory = CurrentSite.StylesheetStorageContainer;

            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetFile(directory, theme.ThemeGuid, name);

            if (!file.Exists())
                throw new PageNotFoundException("The requested resource could not be found. " + name);

            return Convert(theme, file);
        }


        public void Delete(CmsTheme theme, string name)
        {
            String directory = CurrentSite.StylesheetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();

            client.Delete(directory, theme.ThemeGuid, name);
        }


        private static CssFile Convert(CmsTheme theme, StorageFile file)
        {
            CssFile temp = new CssFile();

            Boolean isEnabled = true;
            String enabled = file.Metadata[GetEnabledKey(theme)];
            Boolean.TryParse(enabled, out isEnabled);

            temp.IsEnabled = isEnabled;
            temp.FullName = file.Filename;

            if ((file.Data != null) && (file.Data.Length > 0))
                temp.Content = Encoding.UTF8.GetString(file.Data);

            return temp;
        }

        private static string GetEnabledKey(CmsTheme theme)
        {
            return "enabled_" + theme.ThemeGuid.Replace("-", "_");
        }

        internal static string Resolve(string content)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.ImagesDirectoryKey);
            Regex pattern = new Regex("(\\.+/)+images/(.*)");
            return pattern.Replace(content, container + "/$2");
        }
    }
}
