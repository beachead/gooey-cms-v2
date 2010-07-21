using System;
using System.Collections.Generic;
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
            LoadPageData(result);

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


        public CmsPage GetPage(Data.Guid pageGuid)
        {
            CmsPageDao dao = new CmsPageDao();
            Boolean approvedOnly = !(CurrentSite.IsStagingHost);

            CmsPage result = dao.FindByPageGuid(pageGuid);
            LoadPageData(result);

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
            LoadPageData(result);

            return result;
        }

        private static void LoadPageData(CmsPage result)
        {
            if (result != null)
            {
                IStorageClient client = GetStorageClient();
                result.Content = client.OpenAsString(CurrentSite.PageStorageDirectory, result.Guid);
                result.Javascript = client.OpenAsString(CurrentSite.JavascriptStorageDirectory, result.Guid);
                result.Stylesheet = client.OpenAsString(CurrentSite.StylesheetStorageDirectory, result.Guid);
            }
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

        public void Remove(CmsPage page)
        {
            CmsPageDao dao = new CmsPageDao();
            using (Transaction tx = new Transaction())
            {
                Cleanup(page.Guid);
                dao.Delete<CmsPage>(page);
                tx.Commit();
            }
        }

        public System.Collections.Generic.IList<CmsPage> Filter(string filter)
        {
            return Filter(CurrentSite.Guid, filter);
        }

        public System.Collections.Generic.IList<CmsPage> Filter(Data.Guid siteGuid, string filter)
        {
            CmsPageDao dao = new CmsPageDao();
            filter = filter.Replace("*", "%");
            return dao.SearchByUrl(siteGuid,filter);
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

        public void RemoveObsoletePages(CmsPage page)
        {
            CmsPageDao dao = new CmsPageDao();

            IList<CmsPage> unapproved = dao.FindUnapprovedPages(CurrentSite.Guid,Data.Hash.New(page.UrlHash));
            IList<CmsPage> approved = dao.FindApprovedPages(CurrentSite.Guid,Data.Hash.New(page.UrlHash));

            IStorageClient client = GetStorageClient();

            //Loop through all of the unapproved pages and remove any old versions.
            //Start at the first one, since we always want to leave the latest unapproved version
            using (Transaction tx = new Transaction())
            {
                for (int i = 1; i < unapproved.Count; i++)
                {
                    client.Delete(CurrentSite.PageStorageDirectory, unapproved[i].Guid);
                    client.Delete(CurrentSite.JavascriptStorageDirectory, unapproved[i].Guid);
                    client.Delete(CurrentSite.StylesheetStorageDirectory, unapproved[i].Guid);      
                    dao.Delete<CmsPage>(unapproved[i]);
                }
                tx.Commit();
            }

            //Loop through all of the approved pages and remove any old versions.
            //Start at the first one, since we always want to leave the latest approved version
            using (Transaction tx = new Transaction())
            {
                for (int i = 1; i < approved.Count; i++)
                {
                    client.Delete(CurrentSite.PageStorageDirectory, unapproved[i].Guid);
                    client.Delete(CurrentSite.JavascriptStorageDirectory, unapproved[i].Guid);
                    client.Delete(CurrentSite.StylesheetStorageDirectory, unapproved[i].Guid);   
                    dao.Delete<CmsPage>(approved[i]);
                }
                tx.Commit();
            }
        }

        public void Remove(Data.Guid guid)
        {
            CmsPage page = this.GetPage(guid);
            this.Remove(page);
        }
    }
}
