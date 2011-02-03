using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Page;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Util;
using Microsoft.Security.Application;
using Gooeycms.Business.Cache;
using Gooeycms.Extensions;

namespace Gooeycms.Business.Storage
{
    public abstract class AssetFileManager<T> where T : SortableAssetFile, new()
    {
        private Data.Model.Page.CmsPage cmsPage;

        public AssetFileManager()
        {
        }

        public AssetFileManager(Data.Model.Page.CmsPage cmsPage)
        {
            this.cmsPage = cmsPage;
        }

        public abstract String AssetExtension { get; }
        public abstract String AssetStorageContainerFormat { get; }
        public abstract String CurrentSiteAssetStorageContainer { get; }
        public abstract String CachePrefix { get; }
        public abstract String GetIncludes(CmsPage page);

        private IList<T> List(String key)
        {
            CacheInstance cache = CurrentSite.Cache;

            IList<T> results = cache.Get<IList<T>>(CachePrefix + key);
            if (results == null)
            {
                results = new List<T>();
                String container = CurrentSiteAssetStorageContainer;
                IStorageClient client = StorageHelper.GetStorageClient();

                IList<StorageFile> files = client.List(container, key);
                foreach (StorageFile file in files)
                {
                    results.Add(Convert(key, file));
                }
                ((List<T>)results).Sort();

                cache.Add(CachePrefix + key, results);
            }
            return results;
        }

        public IList<T> List(CmsPage cmsPage)
        {
            return List(cmsPage.UrlHash);
        }

        /// <summary>
        /// Returns a list of the current asset files
        /// currently associated with this theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public IList<T> List(CmsTheme theme)
        {
            return List(theme.ThemeGuid);
        }

        public void Save(Data.Guid siteGuid, CmsPage page, T file)
        {
            String key = page.UrlHash;
            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), file.IsEnabled.StringValue());

            String container = String.Format(AssetStorageContainerFormat, siteGuid.Value);
            client.Save(container, key, file.FullName, Encoding.UTF8.GetBytes(file.Content), Permissions.Private);
        }

        public void Save(Data.Guid siteGuid, CmsTheme theme, T file)
        {
            String key = theme.ThemeGuid;
            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), file.IsEnabled.StringValue());

            String container = String.Format(AssetStorageContainerFormat,siteGuid.Value);
            client.Save(container, key, file.FullName, Encoding.UTF8.GetBytes(file.Content), Permissions.Private);
        }

        public void Save(String key, string filename, byte[] data, Boolean enabledByDefault, Int32 defaultSortOrder)
        {
            CurrentSite.Cache.Clear(CachePrefix + key);
            if (!filename.EndsWith(AssetExtension))
            {
                filename = filename + AssetExtension;
            }

            String enabled = enabledByDefault.StringValue();
            int sortOrder = defaultSortOrder;
            try
            {
                T exists = Get(key, filename);
                enabled = (exists.IsEnabled) ? Boolean.TrueString : Boolean.FalseString;
                sortOrder = exists.SortOrder;
            }
            catch (FileDoesNotExistException) { }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), enabled);
            client.AddMetadata(GetSortKey(key), sortOrder.ToString()); 

            String container = CurrentSiteAssetStorageContainer;
            client.Save(container, key, filename, data, Permissions.Private);

            if (enabledByDefault)
                SitePageCacheRefreshInvoker.InvokeRefresh(CurrentSite.Guid.Value, SitePageRefreshRequest.PageRefreshType.Staging);
        }

        public void Save(CmsPage page, string filename, byte[] data)
        {
            Save(page.UrlHash, filename, data, true,0);
        }

        public void Save(CmsTheme theme, string filename, byte[] data)
        {
            Save(theme.ThemeGuid, filename, data, false,0);
        }

        private void UpdateSortInfo(String key, String filename, Int32 newSortOrder)
        {
            CurrentSite.Cache.Clear(CachePrefix + key);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetSortKey(key), newSortOrder.ToString());

            String container = CurrentSiteAssetStorageContainer;
            client.SetMetadata(container, key, filename);

            SitePageCacheRefreshInvoker.InvokeRefresh(CurrentSite.Guid.Value, SitePageRefreshRequest.PageRefreshType.Staging);
        }

        public void UpdateSortInfo(CmsTheme theme, String filename, Int32 newSortOrder)
        {
            UpdateSortInfo(theme.ThemeGuid, filename, newSortOrder);
        }

        public void UpdateSortInfo(CmsPage page, String filename, Int32 newSortOrder)
        {
            UpdateSortInfo(page.UrlHash, filename, newSortOrder);
        }

        private void EnableDisable(String key, String filename, Boolean enabled)
        {
            CurrentSite.Cache.Clear(CachePrefix + key);

            String enabledString = (enabled) ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            IStorageClient client = StorageHelper.GetStorageClient();
            client.AddMetadata(GetEnabledKey(key), enabledString);

            String container = CurrentSiteAssetStorageContainer;
            client.SetMetadata(container, key, filename);

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

        public T Get(String key, String name)
        {
            String container = CurrentSiteAssetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();
            StorageFile file = client.GetFile(container, key, name);

            if (!file.Exists())
                throw new FileDoesNotExistException("The requested resource could not be found. " + name);

            return Convert(key, file);
        }

        public T Get(CmsTheme theme, string name)
        {
            return Get(theme.ThemeGuid, name);
        }

        public T Get(CmsPage page, string name)
        {
            return Get(page.UrlHash, name);
        }

        public byte[] GetData(String container, String directory, String filename)
        {
            IStorageClient client = StorageHelper.GetStorageClient();
            return client.Open(container, directory, filename);
        }

        /// <summary>
        /// Renames all of the specified assets to the new page
        /// </summary>
        /// <param name="oldpage"></param>
        /// <param name="newpage"></param>
        public void Rename(CmsPage oldpage, CmsPage newpage)
        {
            String container = CurrentSiteAssetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();

            IList<T> oldfiles = List(oldpage);
            
            //Upload to the new directory
            foreach (T oldfile in oldfiles)
            {
                byte[] data = GetData(container, oldpage.UrlHash, oldfile.FullName);
                Save(newpage, oldfile.FullName, data);
            }

            //Delete the old files
            foreach (T oldfile in oldfiles)
            {
                Delete(container, oldpage.UrlHash, oldfile.FullName);
            }
        }

        public Boolean ContainsSnapshots(CmsTheme theme)
        {
            String container = CurrentSiteAssetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();
            return (client.ContainsSnapshots(container, theme.ThemeGuid));
        }

        public Boolean ContainsSnapshots(CmsPage page)
        {
            String container = CurrentSiteAssetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();
            return (client.ContainsSnapshots(container, page.UrlHash));
        }

        public void DeleteAll(CmsTheme theme)
        {
            String container = CurrentSiteAssetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();
            client.Delete(container, theme.ThemeGuid);
        }

        public void Delete(CmsPage page, string name)
        {
            String container = CurrentSiteAssetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();

            client.Delete(container, page.UrlHash, name);
            SitePageCacheRefreshInvoker.InvokeRefresh(page.SubscriptionId, SitePageRefreshRequest.PageRefreshType.Staging);

            if (CurrentSite.IsAvailable)
                CurrentSite.Cache.Clear(CachePrefix + page.UrlHash);
        }

        public void Delete(CmsTheme theme, string name)
        {
            String container = CurrentSiteAssetStorageContainer;
            IStorageClient client = StorageHelper.GetStorageClient();

            client.Delete(container, theme.ThemeGuid, name);
            SitePageCacheRefreshInvoker.InvokeRefresh(theme.SubscriptionGuid, SitePageRefreshRequest.PageRefreshType.Staging);

            if (CurrentSite.IsAvailable)
                CurrentSite.Cache.Clear(CachePrefix + theme.ThemeGuid);
        }

        public void Delete(String container, String directory, String filename)
        {
            IStorageClient client = StorageHelper.GetStorageClient();
            client.Delete(container, directory, filename);
        }

        private static T Convert(String key, StorageFile file)
        {
            T temp = new T();

            Int32 sortOrder = 0;
            Boolean isEnabled = true;
            String enabled = file.Metadata[GetEnabledKey(key)];
            String sortOrderStr = file.Metadata[GetSortKey(key)];

            Boolean.TryParse(enabled, out isEnabled);
            Int32.TryParse(sortOrderStr, out sortOrder);

            temp.IsEnabled = isEnabled;
            temp.FullName = file.Filename;
            temp.SortOrder = sortOrder;
            temp.LastModified = file.LastModified;
            if ((file.Data != null) && (file.Data.Length > 0))
                temp.Content = Encoding.UTF8.GetString(file.Data);

            return temp;
        }

        private static string GetEnabledKey(String key)
        {
            return "enabled_" + key.Replace("-", "_");
        }

        private static String GetSortKey(String key)
        {
            return "sort_" + key.Replace("-", "_");
        }
    }
}
