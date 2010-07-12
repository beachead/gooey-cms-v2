using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Site;
using Gooeycms.Business.Crypto;

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

        public void Save(CmsPage page)
        {
            IStorageClient storage = new AzureBlobStorageClient();
            storage.Save(CurrentSite.PageStorageKey,page.Guid,"This is a test");
        }

        /// <summary>
        /// Gets the latest page baesd upon an encrypted primary key
        /// </summary>
        /// <param name="encryptedPageId"></param>
        /// <returns></returns>
        public CmsPage GetPage(String encryptedPageId)
        {
            CmsPage result = null;

            int id = 0;
            String temp = TextEncryption.Decode(encryptedPageId);
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

        public CmsPage GetLatestPage(String siteGuid, String pageGuid)
        {
            CmsPageDao dao = new CmsPageDao();
            Boolean approvedOnly = !(CurrentSite.IsStagingHost);

            CmsPage result = dao.FindBySiteAndGuid(siteGuid, pageGuid);
            if (CurrentSite.IsProductionHost)
            {
                if (!result.IsApproved)
                {
                    Logging.Info("A request was made for the site:" + siteGuid + "'s page: " + pageGuid + ", however, this page is not approved and will not be returned.");
                    result = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the latest page based upon the path
        /// </summary>
        /// <param name="guid">The site guid</param>
        /// <param name="path">The page path</param>
        /// <returns></returns>
        public CmsPage GetLatestPage(String guid, CmsUrl uri)
        {
            String path = uri.Path;
            CmsPageDao dao = new CmsPageDao();

            Boolean approvedOnly = !(CurrentSite.IsStagingHost);
            CmsPage result = dao.FindLatesBySiteAndPath(guid, path, approvedOnly);

            //Check if there's a default page that should be loaded
            if (result == null)
            {
                String separator = CmsSiteMap.PathSeparator;
                if (path.EndsWith(CmsSiteMap.PathSeparator))
                    separator = String.Empty;

                String pathWithDefault = path + separator + CmsSiteMap.DefaultPageName;
                result = dao.FindLatesBySiteAndPath(guid, pathWithDefault, approvedOnly);
            }

            return result;
        }

        /// <summary>
        /// Gets the latest page for the current site and path.
        /// </summary>
        /// <param name="path"></param>
        public CmsPage GetLatestPage(string path)
        {
            CmsUrl uri = CmsUrl.Parse(path);
            return GetLatestPage(CurrentSite.Guid, uri);
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

            try
            {
                //CmsSiteMap.Instance.AddPath(
            }
            catch (Exception ex)
            {
                Logging.Error("There was an unexpected exception adding the page", ex);
                //TODO Perform rollback
            }
        }
    }
}
