using System;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Page;
using Gooeycms.Data.Model.Site;

namespace Gooeycms.Business.Pages
{
    public class PageManager
    {
        private static PageManager instance = new PageManager();
        private PageManager() { }
        public static PageManager Instance 
        {
            get { return PageManager.instance; }
        }

        /// <summary>
        /// Gets the latest page baesd upon an encrypted primary key
        /// </summary>
        /// <param name="encryptedPageId"></param>
        /// <returns></returns>
        public CmsPage GetPage(Data.EncryptedValue encryptedPageId)
        {
            CmsPage result = null;

            int id = 0;
            String temp = TextEncryption.Decode(encryptedPageId.Value);
            Int32.TryParse(temp, out id);

            CmsPageDao dao = new CmsPageDao();
            result = dao.FindByPrimaryKey<CmsPage>(id);
            if (CurrentSite.IsProductionHost)
            {
                if (!result.IsApproved)
                {
                    Logging.Info("A request for the page (primary key=" + id + ") has not been approved and will not be returned in production mode.");
                    result = null;
                }
            }

            return result;
        }


        public CmsPage GetLatestPage(Data.Guid pageGuid)
        {
            CmsPageDao dao = new CmsPageDao();
            Boolean approvedOnly = !(CurrentSite.IsStagingHost);

            CmsPage result = dao.FindByPageGuid(pageGuid);
            if (CurrentSite.IsProductionHost)
            {
                if (!result.IsApproved)
                {
                    Logging.Info("A request was made for page: " + pageGuid + ", (owner=" + result.SubscriptionId +") however, this page is not approved and will not be returned.");
                    result = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the latest page for the current site and path.
        /// </summary>
        /// <param name="path"></param>
        public CmsPage GetLatestPage(CmsUrl uri)
        {
            return GetLatestPage(CurrentSite.Guid, uri);
        }

        /// <summary>
        /// Gets the latest page based upon the path
        /// </summary>
        /// <param name="siteGuid">The site guid</param>
        /// <param name="path">The page path</param>
        /// <returns></returns>
        public CmsPage GetLatestPage(Data.Guid siteGuid, CmsUrl uri)
        {
            String path = uri.Path;
            Data.Hash pathHash = TextHash.MD5(path);

            CmsPageDao dao = new CmsPageDao();

            Boolean approvedOnly = !(CurrentSite.IsStagingHost);
            CmsPage result = dao.FindLatesBySiteAndHash(siteGuid, pathHash, approvedOnly);

            //Check if there's a default page that should be loaded
            if (result == null)
            {
                String separator = CmsSiteMap.PathSeparator;
                if (path.EndsWith(CmsSiteMap.PathSeparator))
                    separator = String.Empty;

                Data.Hash hashWithDefault = TextHash.MD5(path + separator + CmsSiteMap.DefaultPageName);
                result = dao.FindLatesBySiteAndHash(siteGuid, hashWithDefault, approvedOnly);
            }

            //Load the page contents
            IStorageClient client = GetStorageClient();
            result.Content = client.OpenAsString(CurrentSite.PageStorageDirectory, result.Guid);
            result.Javascript = client.OpenAsString(CurrentSite.JavascriptStorageDirectory, result.Guid);
            result.Stylesheet = client.OpenAsString(CurrentSite.StylesheetStorageDirectory, result.Guid);

            return result;
        }

        /// <summary>
        /// Adds a new page to the CMS system.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pageName"></param>
        /// <param name="page"></param>
        public void AddNewPage(string parent, string pageName, CmsPage page)
        {
            String fullurl = CmsSiteMap.PathCombine(parent, pageName);
            page.Guid = System.Guid.NewGuid().ToString();
            page.Url = fullurl;
            page.UrlHash = TextHash.MD5(page.Url).Value;

            CmsPageDao dao = new CmsPageDao();
            CmsSitePath path =  null;
            IStorageClient client = GetStorageClient();
            try
            {
                path = CmsSiteMap.Instance.AddNewPage(parent, pageName);

                using (Transaction tx = new Transaction())
                {
                    dao.Save<CmsPage>(page);
                    tx.Commit();
                }

                Cleanup(page.Guid);
                client.Save(CurrentSite.PageStorageDirectory, page.Guid, page.Content);
                client.Save(CurrentSite.JavascriptStorageDirectory, page.Guid, page.Javascript);
                client.Save(CurrentSite.StylesheetStorageDirectory, page.Guid, page.Stylesheet);
            }
            catch (Exception ex)
            {
                Logging.Error("There was an unexpected exception adding the page", ex);
                
                CmsSiteMap.Instance.Remove(path);
                this.Remove(page);
                client.Delete(CurrentSite.PageStorageDirectory, page.Guid);

                throw;
            }
        }

        /// <summary>
        /// Cleans up the storage client to prevent any potential issues
        /// </summary>
        /// <param name="p"></param>
        private void Cleanup(string guid)
        {
            IStorageClient client = GetStorageClient();
            client.Delete(CurrentSite.PageStorageDirectory, guid);
            client.Delete(CurrentSite.JavascriptStorageDirectory, guid);
            client.Delete(CurrentSite.StylesheetStorageDirectory, guid);
        }

        internal static IStorageClient GetStorageClient()
        {
            IStorageClient client = new AzureBlobStorageClient();
            return client;
        }

        public void Remove(CmsPage page)
        {
            CmsPageDao dao = new CmsPageDao();
            using (Transaction tx = new Transaction())
            {
                dao.Delete<CmsPage>(page);
                tx.Commit();
            }
        }
    }
}
