using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Site;

namespace Gooeycms.Business.Site
{
    public class CmsSiteMap
    {
        public const String PathSeparator = "/";

        private static CmsSiteMap instance = new CmsSiteMap();
        private CmsSiteMap() { }
        public static CmsSiteMap Instance
        {
            get { return CmsSiteMap.instance; }
        }

        /// <summary>
        /// Gets the parent paths of the current site
        /// </summary>
        /// <returns></returns>
        public IList<String> GetParentPaths()
        {
            List<String> results = new List<String>();
            results.Add("/");
            results.Add("/path1/");
            results.Add("/path2/");

            return results;
        }

        public CmsSitePath GetPath(String siteGuid, String path)
        {
            CmsSitePathDao dao = new CmsSitePathDao();
            return dao.FindBySiteAndPath(siteGuid, path);
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
    }
}
