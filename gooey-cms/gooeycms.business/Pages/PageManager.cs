using System;
using System.Collections.Generic;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Business.Web;
using Gooeycms.Data.Model.Page;
using Gooeycms.Data.Model.Site;
using System.Threading.Tasks;

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
            return GetPage(pageGuid, true);
        }

        public CmsPage GetPage(Data.Guid pageGuid, Boolean loadData)
        {
            CmsPageDao dao = new CmsPageDao();
            Boolean approvedOnly = !(CurrentSite.IsStagingHost);

            CmsPage result = dao.FindByPageGuid(pageGuid);
            if (loadData)
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
            return GetLatestPage(CurrentSite.Guid, uri, true);
        }

        public CmsPage GetLatestPage(Data.Guid siteGuid, CmsUrl uri)
        {
            return GetLatestPage(siteGuid, uri, true);
        }

        public CmsPage GetLatestPage(CmsUrl uri, bool loadData)
        {
            return GetLatestPage(CurrentSite.Guid, uri, loadData);
        }

        /// <summary>
        /// Gets the latest page based upon the path
        /// </summary>
        /// <param name="siteGuid">The site guid</param>
        /// <param name="path">The page path</param>
        /// <returns></returns>
        public CmsPage GetLatestPage(Data.Guid siteGuid, CmsUrl uri, bool loadData)
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
            if (loadData)
                LoadPageData(result);

            return result;
        }

        private static void LoadPageData(CmsPage result)
        {
            if (result != null)
            {
                IStorageClient client = StorageHelper.GetStorageClient();
                result.Content = client.OpenAsString(CurrentSite.PageStorageContainer, StorageClientConst.RootFolder, result.Guid);
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
            CmsPageDao dao = new CmsPageDao();
            CmsSitePath path =  null;
            IStorageClient client = StorageHelper.GetStorageClient();
            try
            {
                path = CmsSiteMap.Instance.AddNewPage(Data.Guid.New(page.SubscriptionId),parent, pageName);

                using (Transaction tx = new Transaction())
                {
                    dao.Save<CmsPage>(page);
                    tx.Commit();
                }
                client.Save(SiteHelper.GetStorageKey(SiteHelper.PageDirectoryKey,page.SubscriptionId), StorageClientConst.RootFolder, page.Guid, page.Content, Permissions.Private);
            }
            catch (Exception ex)
            {
                Logging.Error("There was an unexpected exception adding the page", ex);
                
                CmsSiteMap.Instance.Remove(path);
                this.Remove(page);
            }
        }

        /// <summary>
        /// Removes all versions of the page
        /// </summary>
        /// <param name="page"></param>
        public void DeleteAll(CmsPage page)
        {
            if (page != null)
            {
                String container = SiteHelper.GetStorageKey(SiteHelper.PageDirectoryKey, page.SubscriptionId);

                IStorageClient client = StorageHelper.GetStorageClient();
                CmsPageDao dao = new CmsPageDao();

                IList<CmsPage> pages = dao.FindAllPages(Data.Guid.New(page.SubscriptionId), Data.Hash.New(page.UrlHash));
                using (Transaction tx = new Transaction())
                {
                    foreach (CmsPage temp in pages)
                    {
                        client.Delete(container, StorageClientConst.RootFolder, temp.Guid);
                        dao.Delete<CmsPage>(temp);
                    }
                    tx.Commit();
                }

                CmsSitePath path = CmsSiteMap.Instance.GetPath(page.Url);
                if (page != null)
                    CmsSiteMap.Instance.Remove(path);
            }
        }

        public void Remove(CmsPage page)
        {
            if (page != null)
            {
                String container = SiteHelper.GetStorageKey(SiteHelper.PageDirectoryKey, page.SubscriptionId);

                IStorageClient client = StorageHelper.GetStorageClient();
                CmsPageDao dao = new CmsPageDao();
                using (Transaction tx = new Transaction())
                {
                    client.Delete(container, StorageClientConst.RootFolder, page.Guid);
                    dao.Delete<CmsPage>(page);
                    tx.Commit();
                }
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

        public void RemoveObsoletePages(CmsPage page)
        {
            CmsPageDao dao = new CmsPageDao();

            IList<CmsPage> unapproved = dao.FindUnapprovedPages(Data.Guid.New(page.SubscriptionId),Data.Hash.New(page.UrlHash));
            IList<CmsPage> approved = dao.FindApprovedPages(Data.Guid.New(page.SubscriptionId), Data.Hash.New(page.UrlHash));

            IStorageClient client = StorageHelper.GetStorageClient();
            String container = SiteHelper.GetStorageKey(SiteHelper.PageDirectoryKey, page.SubscriptionId);

            //Loop through all of the unapproved pages and remove any old versions.
            //Start at the first one, since we always want to leave the latest unapproved version
            using (Transaction tx = new Transaction())
            {
                for (int i = 1; i < unapproved.Count; i++)
                {
                    client.Delete(container, StorageClientConst.RootFolder, unapproved[i].Guid);   
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
                    client.Delete(container, StorageClientConst.RootFolder, approved[i].Guid);
                    dao.Delete<CmsPage>(approved[i]);
                }
                tx.Commit();
            }
        }

        public void Remove(Data.Guid guid)
        {
            CmsPage page = this.GetPage(guid, false);
            this.Remove(page);
        }

        public static void ValidateMarkup(string p)
        {
        }

        public static void PublishToWorker(CmsPage page,Gooeycms.Business.Pages.PageTaskMessage.Actions action)
        {
            PageTaskMessage message = new PageTaskMessage();
            message.Action = action;
            message.Page = page;

            QueueManager queue = new QueueManager(QueueNames.PageActionQueue);
            queue.Put<PageTaskMessage>(message, TimeSpan.FromMinutes(60));
        }
    }
}
