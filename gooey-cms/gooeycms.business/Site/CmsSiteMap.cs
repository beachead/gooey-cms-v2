using System;
using System.Collections.Generic;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Site;

namespace Gooeycms.Business.Web
{
    public class CmsSiteMap
    {
        public const String PathSeparator = "/";
        public const String RootPath = PathSeparator;

        private static CmsSiteMap instance = new CmsSiteMap();
        
        private CmsSiteMap() { }
        public static CmsSiteMap Instance
        {
            get { return CmsSiteMap.instance; }
        }

        public IList<CmsSitePath> GetParentPaths(Data.Guid siteGuid)
        {
            CmsSitePathDao dao = new CmsSitePathDao();
            return dao.FindParentPathsBySite(siteGuid);
        }

        /// <summary>
        /// Gets the parent paths of the current site
        /// </summary>
        /// <returns></returns>
        public IList<CmsSitePath> GetParentPaths()
        {
            return GetParentPaths(CurrentSite.Guid);
        }

        public CmsSitePath GetPath(Data.Guid siteGuid, String path)
        {
            if (path == null)
                throw new ArgumentNullException("The path may not be null when attempting to retrieve a sitemap path");

            Data.Hash hash = TextHash.MD5(path);

            CmsSitePathDao dao = new CmsSitePathDao();
            return dao.FindBySiteAndHash(siteGuid, hash);
        }

        public CmsSitePath GetPath(string path)
        {
            return GetPath(CurrentSite.Guid, path);
        }

        public static string PathCombine(string parent, string page)
        {
            String slash = "";
            if (!parent.EndsWith("/"))
                slash = "/";

            return parent + slash + page;
        }

        public static String DefaultPageName
        {
            get { return GooeyConfigManager.DefaultPageName; }
        }

        public CmsSitePath GetRootPath(Data.Guid siteGuid)
        {
            return GetPath(siteGuid, CmsSiteMap.RootPath);
        }

        public CmsSitePath GetRootPath()
        {
            return GetRootPath(CurrentSite.Guid);   
        }

        public CmsSitePath AddRootDirectory(Data.Guid siteGuid)
        {
            CmsSitePath path = GetRootPath(siteGuid);
            if (path == null)
            {
                path = new CmsSitePath();
                path.SubscriptionGuid = siteGuid.Value;
                path.Parent = null;
                path.IsDirectory = true;
                path.Depth = 1;
                path.Position = 0;
                path.Url = RootPath;
                path.UrlHash = TextHash.MD5(path.Url).Value;

                ValidatePath(siteGuid, path.Url);
                CmsSitePathDao dao = new CmsSitePathDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Save<CmsSitePath>(path);
                    tx.Commit();
                }
            }
            return path;
        }


        public CmsSitePath AddRootDirectory()
        {
            return AddRootDirectory(CurrentSite.Guid);
        }

        public CmsSitePath AddChildDirectory(Data.Guid siteGuid, String parentPath, String newDirectory)
        {
            CmsSitePath parent = GetPath(siteGuid, parentPath);
            if (parent == null)
                throw new ArgumentException("Could not add child directory because the parent path '" + parentPath + "' does not exist.");

            String slash = (parent.Url.EndsWith("/")) ? "" : "/";
            CmsSitePath path = new CmsSitePath();

            path.SubscriptionGuid = siteGuid.Value;
            path.Parent = parent.Url;
            path.IsDirectory = true;
            path.Depth = parent.Depth + 1;
            path.Url = parent.Url + slash + newDirectory;
            path.UrlHash = TextHash.MD5(path.Url).Value;
            path.Position = GetNextPosition(siteGuid,path.Depth);

            ValidatePath(siteGuid, path.Url);
            CmsSitePathDao dao = new CmsSitePathDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsSitePath>(path);
                tx.Commit();
            }

            return path;
        }

        public CmsSitePath AddChildDirectory(String parentPath, String newDirectory)
        {
            return AddChildDirectory(CurrentSite.Guid, parentPath, newDirectory);
        }

        public CmsSitePath AddNewPage(Data.Guid siteGuid, String parentPath, String newPage)
        {
            CmsSitePath parent = GetPath(siteGuid, parentPath);
            if (parent == null)
                throw new ArgumentException(parentPath + " does not exist and can not be used as a parent for " + newPage);

            String fullurl = PathCombine(parent.Url, newPage);

            //Make sure the url doesn't already exist
            CmsSitePath path = GetPath(siteGuid, fullurl);
            if (path == null)
            {
                int depth = parent.Depth + 1;

                path = new CmsSitePath();
                path.SubscriptionGuid = siteGuid.Value;
                path.Depth = depth;
                path.Position = GetNextPosition(siteGuid, depth);
                path.Parent = parent.Url;
                path.Url = fullurl;
                path.UrlHash = TextHash.MD5(fullurl).Value;
                path.IsPage = true;

                CmsSitePathDao dao = new CmsSitePathDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Save<CmsSitePath>(path);
                    tx.Commit();
                }
            }
            return path;
        }

        internal void AddNewPage(Data.Guid guid, string fullUrl)
        {
            int pos = fullUrl.LastIndexOf("/");
            String parent = fullUrl.Substring(0, pos + 1);
            String page = fullUrl.Substring(pos + 1);

            AddNewPage(guid,parent,page);
        }

        public CmsSitePath AddNewPage(string parentPath, string path)
        {
            return AddNewPage(CurrentSite.Guid, parentPath, path);
        }

        public bool Exists(Data.Guid siteGuid, String fullurl)
        {
            return (GetPath(siteGuid, fullurl) != null);
        }

        public bool Exists(string fullurl)
        {
            return Exists(CurrentSite.Guid, fullurl);
        }

        public void Remove(CmsSitePath path)
        {
            CmsSitePathDao dao = new CmsSitePathDao();
            using (Transaction tx = new Transaction())
            {
                dao.Delete <CmsSitePath>(path);
                tx.Commit();
            }
        }

        /// <summary>
        /// Gets the next available position for the specified depth
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        private int GetNextPosition(Data.Guid siteGuid, int depth)
        {
            CmsSitePathDao dao = new CmsSitePathDao();
            return dao.FindNextPosition(siteGuid, depth);
        }

        private void ValidatePath(Data.Guid siteGuid, String path)
        {
            bool exists = Exists(siteGuid, path);
            if (exists)
                throw new ArgumentException("The specified path '" + path + "' already exists and may not be used again.");

            //Regex pattern = new Regex("^(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\+&amp;%\$#_]*)?$",RegexOptions.IgnoreCase | RegexOptions.Compiled);

        }

        public IList<CmsSitePath> GetAllPaths(Data.Guid siteGuid)
        {
            CmsSitePathDao dao = new CmsSitePathDao();
            return dao.FindAllBySiteGuid(siteGuid);
        }

        public void Save(CmsSitePath path)
        {
            CmsSitePathDao dao = new CmsSitePathDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsSitePath>(path);
                tx.Commit();
            }
        }
    }
}
