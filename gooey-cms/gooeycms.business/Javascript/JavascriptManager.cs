using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Gooeycms.Business.Pages;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;
using Microsoft.Security.Application;

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
            // TODO: Complete member initialization
            this.cmsPage = cmsPage;
        }

        public String GetJavascriptIncludes()
        {
            StringBuilder includes = new StringBuilder();
            IList<JavascriptFile> scripts = List(CurrentSite.GetCurrentTheme());
            foreach (JavascriptFile script in scripts)
            {
                if (script.IsEnabled)
                    includes.AppendLine("<script src=\"~/gooeyscripts/" + AntiXss.UrlEncode(script.FullName) + "\" type=\"text/javascript\" language=\"javascript\"></script>");
            }

            return includes.ToString();
        }

        /// <summary>
        /// Returns a list of the current javascript files
        /// currently associated with this theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<JavascriptFile> List(CmsTheme theme)
        {
            String directory = CurrentSite.JavascriptStorageDirectory;
            IStorageClient client = StorageHelper.GetStorageClient();

            IList<StorageFile> files = client.List(directory);
            IList<JavascriptFile> results = new List<JavascriptFile>();
            foreach (StorageFile file in files)
            {
                results.Add(Convert(theme,file));
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
            if (!filename.EndsWith(JavascriptFile.Extension))
            {
                filename = filename + JavascriptFile.Extension;
            }  

            String directory = CurrentSite.JavascriptStorageDirectory;
            filename = GetRelativeFilename(theme, filename);

            String enabled = "false";
            try
            {
                JavascriptFile exists = Get(theme, filename);
                enabled = (exists.IsEnabled) ? Boolean.TrueString : Boolean.FalseString;
            }
            catch (PageNotFoundException) { }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), enabled);

            client.Save(directory, filename, data, Permissions.Private);
        }

        /// <summary>
        /// Enables the specified javascript file to run on the theme
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="filename"></param>
        public void Enable(CmsTheme theme, string filename)
        {
            String directory = CurrentSite.JavascriptStorageDirectory;
            String actualFilename = GetRelativeFilename(theme, filename);
            
            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), "true");
            client.SetMetadata(directory,actualFilename);
        }

        public void Disable(CmsTheme theme, String filename)
        {
            String directory = CurrentSite.JavascriptStorageDirectory;
            String actualFilename = GetRelativeFilename(theme, filename);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(theme), "false");
            client.SetMetadata(directory, actualFilename);
        }

        public static String GetRelativeFilename(CmsTheme theme, String filename)
        {
            return filename;
        }

        public JavascriptFile Get(CmsTheme theme, string name)
        {
            String directory = CurrentSite.JavascriptStorageDirectory;

            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetFile(directory, name);

            if (!file.Exists())
                throw new PageNotFoundException("The requested resource could not be found. " + name);

            return Convert(theme, file);
        }


        public void Delete(CmsTheme cmsTheme, string name)
        {
            String directory = CurrentSite.JavascriptStorageDirectory;
            IStorageClient client = StorageHelper.GetStorageClient();

            client.Delete(directory, name);
        }


        private static JavascriptFile Convert(CmsTheme theme, StorageFile file)
        {
            JavascriptFile temp = new JavascriptFile();

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
            return "enabled_" + theme.ThemeGuid.Replace("-","_");
        }
    }
}
