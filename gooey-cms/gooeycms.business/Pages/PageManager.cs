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
using Gooeycms.Business.Javascript;
using Gooeycms.Business.Css;
using Gooeycms.Business.Subscription;
using System.Text.RegularExpressions;

namespace Gooeycms.Business.Pages
{
    public class PageManager
    {
        public static class Filters
        {
            public const String AllPages = null;
        }

        public static Regex JavascriptPattern = new Regex(@"<\s*script\s*.*?>", RegexOptions.IgnoreCase);

        private static PageManager instance = new PageManager();
        private PageManager() { }
        public static PageManager Instance 
        {
            get { return PageManager.instance; }
        }

        public IList<CmsPage> GetUnapprovedPages(Data.Guid siteGuid)
        {
            CmsPageDao dao = new CmsPageDao();
            return dao.FindUnapprovedPages(siteGuid);
        }

        public IList<CmsPage> GetUnapprovedPages()
        {
            return GetUnapprovedPages(CurrentSite.Guid);
        }

        public IList<CmsPage> GetPages(Data.Guid siteGuid, CmsUrl url)
        {
            CmsPageDao dao = new CmsPageDao();
            return dao.FindByPageHash(siteGuid, TextHash.MD5(url.ToString()));
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
            return GetPage(CurrentSite.Guid, pageGuid, loadData);
        }

        public CmsPage GetPage(Data.Guid siteGuid, Data.Guid pageGuid, Boolean loadData)
        {
            CmsPageDao dao = new CmsPageDao();
            Boolean approvedOnly = !(CurrentSite.IsStagingHost);

            CmsPage result = dao.FindByPageGuid(siteGuid, pageGuid);
            if (loadData)
                LoadPageData(result);

            if (CurrentSite.IsProductionHost)
            {
                if (!result.IsApproved)
                {
                    Logging.Info("A request was made for page: " + pageGuid + ", (owner=" + result.SubscriptionId + ") however, this page is not approved and will not be returned.");
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

        public CmsPage GetLatestPage(Data.Guid siteGuid, CmsUrl uri, bool loadData)
        {
            return GetLatestPage(siteGuid, uri, loadData, false);
        }

        /// <summary>
        /// Gets the latest page based upon the path
        /// </summary>
        /// <param name="siteGuid">The site guid</param>
        /// <param name="path">The page path</param>
        /// <returns></returns>
        public CmsPage GetLatestPage(Data.Guid siteGuid, CmsUrl uri, bool loadData, bool forceLatest)
        {
            String path = uri.Path;
            Data.Hash pathHash = TextHash.MD5(path);

            CmsPageDao dao = new CmsPageDao();

            Boolean approvedOnly = false;
            if (!forceLatest)
                approvedOnly = !(CurrentSite.IsStagingHost);

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

        public static void LoadPageData(CmsPage result)
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
            try
            {
                path = CmsSiteMap.Instance.AddNewPage(Data.Guid.New(page.SubscriptionId),parent, pageName);
                Save(page);
            }
            catch (Exception ex)
            {
                Logging.Error("There was an unexpected exception adding the page", ex);
                
                CmsSiteMap.Instance.Remove(path);
                this.Remove(page);
            }
        }

        public void Delete(Data.Guid pageGuid)
        {
            Delete(CurrentSite.Guid, pageGuid);
        }

        /// <summary>
        /// Deletes a specific page from the system
        /// </summary>
        /// <param name="pageGuid"></param>
        public void Delete(Data.Guid siteGuid, Data.Guid pageGuid)
        {
            String container = SiteHelper.GetStorageKey(SiteHelper.PageContainerKey, siteGuid.Value);
            IStorageClient client = StorageHelper.GetStorageClient();
            CmsPageDao dao = new CmsPageDao();

            CmsPage page = GetPage(pageGuid, false);
            if (page != null)
            {
                if (!page.SubscriptionId.Equals(siteGuid.Value))
                    throw new ArgumentException("This page does not belong to the current subscription and can not be retrieved.");

                client.Delete(container, StorageClientConst.RootFolder, page.Guid);
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsPage>(page);
                    tx.Commit();
                }

                //Check if there are other versions of this page, if not, delete the sitepath as well
                IList<CmsPage> pages = GetPages(siteGuid, page.Url);
                if ((pages == null) || (pages.Count == 0))
                {
                    CmsSitePath path = CmsSiteMap.Instance.GetPath(page.Url);
                    if (path != null)
                        CmsSiteMap.Instance.Remove(path);
                }
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
                String container = SiteHelper.GetStorageKey(SiteHelper.PageContainerKey, page.SubscriptionId);

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
            }

            CmsSitePath path = CmsSiteMap.Instance.GetPath(page.Url);
            if (path != null)
                CmsSiteMap.Instance.Remove(path);
        }

        public void Remove(CmsPage page)
        {
            if (page != null)
            {
                String container = SiteHelper.GetStorageKey(SiteHelper.PageContainerKey, page.SubscriptionId);

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
            if (String.IsNullOrWhiteSpace(filter))
                filter = "*";

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
            String container = SiteHelper.GetStorageKey(SiteHelper.PageContainerKey, page.SubscriptionId);

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

        public static Boolean IsAddPageAvailable()
        {
            Boolean result = true;
            if (CurrentSite.IsAvailable)
            {
                IList<CmsPage> pages = PageManager.Instance.Filter(CurrentSite.Guid, PageManager.Filters.AllPages);
                if ((pages.Count) >= (CurrentSite.Restrictions.MaxAllowedPages))
                    result = false;
            }
            return result;
        }

        public static void Validate(CmsPage page, Boolean checkPageCount)
        {
            if (CurrentSite.IsAvailable)
            {
                if (checkPageCount)
                {
                    if (!IsAddPageAvailable())
                        throw new SubscriptionRestrictionException("This subscription has reached its max number of allowed pages of " + CurrentSite.Restrictions.MaxAllowedPages, SubscriptionRestrictionException.RestrictionType.MaxPageCountReached);
                }

                if (!CurrentSite.Restrictions.IsJavascriptAllowed)
                {
                    if (JavascriptPattern.IsMatch(page.Content))
                        throw new SubscriptionRestrictionException("Script tags are not allowed with this subscription type.", SubscriptionRestrictionException.RestrictionType.IllegalScriptTags);
                }
            }
        }

        public static void PublishToWorker(CmsPage page,Gooeycms.Business.Pages.PageTaskMessage.Actions action)
        {
            PageTaskMessage message = new PageTaskMessage();
            message.Action = action;
            message.Page = page;

            QueueManager queue = new QueueManager(QueueNames.PageActionQueue);
            queue.Put<PageTaskMessage>(message, TimeSpan.FromMinutes(60));
        }

        public void Save(CmsPage page)
        {
            CmsPageDao dao = new CmsPageDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsPage>(page);
                tx.Commit();
            }

            IStorageClient client = StorageHelper.GetStorageClient();
            client.Save(SiteHelper.GetStorageKey(SiteHelper.PageContainerKey, page.SubscriptionId), StorageClientConst.RootFolder, page.Guid, page.Content, Permissions.Private);
        }

        public static void CreateDefaultPage(string subscriptionGuid, string templateName)
        {
            String fullurl = CmsSiteMap.PathCombine("/", GooeyConfigManager.DefaultPageName);
            CmsPage page = new CmsPage();
            page.SubscriptionId = subscriptionGuid;
            page.Content = GooeyConfigManager.DefaultHomepage;
            page.Culture = GooeyConfigManager.DefaultCulture;
            page.DateSaved = UtcDateTime.Now;
            page.Description = "GooeyCMS Default Homepage";
            page.Template = templateName;
            page.Title = "Default Homepage";
            page.Url = fullurl;
            page.UrlHash = TextHash.MD5(page.Url).Value;
            page.Guid = System.Guid.NewGuid().ToString();
            page.IsApproved = true;
            page.ApprovedBy = "System";
            PageManager.Instance.AddNewPage("/", GooeyConfigManager.DefaultPageName, page);
        }

        public void Rename(Data.Guid siteGuid, String oldpath, String newpath)
        {
            String container = SiteHelper.GetStorageKey(SiteHelper.PageContainerKey, siteGuid.Value);

            IList<CmsPage> pages = GetPages(siteGuid, oldpath);
            if (pages.Count > 0)
            {
                CmsPage page = pages[0];

                //Update the sitemap
                CmsSitePath parentPath = CmsSiteMap.Instance.GetParentPath(newpath);
                CmsSitePath pagePath = CmsSiteMap.Instance.GetPath(oldpath);
                pagePath.Depth = parentPath.Depth + 1;
                pagePath.Parent = parentPath.Url;
                pagePath.Url = newpath;
                pagePath.UrlHash = TextHash.MD5(pagePath.Url).Value;
                CmsSiteMap.Instance.Save(pagePath);

                //Update the page reference in the database
                page.Url = pagePath.Url;
                page.UrlHash = pagePath.UrlHash;
                Save(page);

                CmsPage newpage = GetLatestPage(page.Url);
                JavascriptManager.Instance.Rename(page, newpage);
                CssManager.Instance.Rename(page, newpage);
            }
        }

        public void Rename(string oldpath, string newpath)
        {
            Rename(CurrentSite.Guid, oldpath, newpath);
        }

        public void DeleteFolder(CmsSitePath folder)
        {
            if (!folder.IsDirectory)
                throw new ArgumentException("This method may only be used to delete folders not pages");

            IList<CmsSitePath> pages = CmsSiteMap.Instance.GetChildren(folder);
            foreach (CmsSitePath path in pages)
            {
                CmsUrl url = new CmsUrl(path.Url);
                CmsPage page = this.GetLatestPage(url, false);
                if (page == null)
                {
                    page = new CmsPage();
                    page.Url = path.Url;
                    page.UrlHash = TextHash.MD5(page.Url).Value;
                }               
                this.DeleteAll(page);
            }

            //Delete the directory itself from the sitemap
            CmsSiteMap.Instance.Remove(folder);
        }

        public void Approve(Data.Guid siteGuid, Data.Guid pageGuid, String approvedBy)
        {
            CmsPage page = GetPage(pageGuid, false);
            if (page != null)
            {
                page.ApprovedBy = approvedBy;
                page.IsApproved = true;
                Save(page);
            }
        }

        public void Approve(Data.Guid guid, string approvedBy)
        {
            Approve(CurrentSite.Guid, guid, approvedBy);
        }
    }
}
