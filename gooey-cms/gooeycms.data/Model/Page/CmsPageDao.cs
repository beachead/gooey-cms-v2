using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Page
{
    public class CmsPageDao : BaseDao
    {
        /// <summary>
        /// Finds the lates page based upon the site guid, page path, and whether it's approved.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="path"></param>
        /// <param name="approvedOnly"></param>
        public CmsPage FindLatesBySiteAndPath(string guid, string path, bool approvedOnly)
        {
            //TODO Code this portion
            return null;
        }

        public CmsPage FindBySiteAndGuid(string siteGuid, string pageGuid)
        {
            //TODO Code this portion
            return null;
        }
    }
}
