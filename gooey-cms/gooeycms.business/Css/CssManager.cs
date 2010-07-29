using System;
using System.Collections.Generic;
using System.Text;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;

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
            // TODO: Complete member initialization
            this.cmsPage = cmsPage;
        }

        [Obsolete("This method is no longer safe to use, because multiple css files may be returned.")]
        internal String GetCssIncludes()
        {
            //TODO: Implement this portion
            return "";
        }

        /// <summary>
        /// Returns a list of the current javascript files
        /// currently associated with this theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<CssFile> List(CmsTheme theme)
        {
            String directory = CurrentSite.StylesheetStorageDirectory;
            IStorageClient client = StorageHelper.GetStorageClient();

            IList<StorageFile> files = client.List(directory);
            IList<CssFile> results = new List<CssFile>();
            foreach (StorageFile file in files)
            {
                results.Add(Convert(theme, file));
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
            if (!filename.EndsWith(CssFile.Extension))
            {
                filename = filename + CssFile.Extension;
            }

            String directory = CurrentSite.StylesheetStorageDirectory;
            String actualFilename = GetRelativeFilename(theme, filename);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), "false");

            client.Save(directory, actualFilename, data, Permissions.Private);
        }

        /// <summary>
        /// Enables the specified javascript file to run on the theme
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="filename"></param>
        public void Enable(CmsTheme theme, string filename)
        {
            String directory = CurrentSite.StylesheetStorageDirectory;
            String actualFilename = GetRelativeFilename(theme, filename);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), "true");
            client.SetMetadata(directory, actualFilename);
        }

        public void Disable(CmsTheme theme, String filename)
        {
            String directory = CurrentSite.StylesheetStorageDirectory;
            String actualFilename = GetRelativeFilename(theme, filename);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), "false");
            client.SetMetadata(directory, actualFilename);
        }

        public static String GetRelativeFilename(CmsTheme theme, String filename)
        {
            return filename;
        }

        public CssFile Get(CmsTheme theme, string name)
        {
            String directory = CurrentSite.StylesheetStorageDirectory;

            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetFile(directory, name);

            if (!file.Exists())
                throw new PageNotFoundException("The requested resource could not be found. " + name);

            return Convert(theme, file);
        }


        public void Delete(CmsTheme cmsTheme, string name)
        {
            String directory = CurrentSite.StylesheetStorageDirectory;
            IStorageClient client = StorageHelper.GetStorageClient();

            client.Delete(directory, name);
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
    }
}
