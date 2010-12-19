using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Util;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Images
{
    public class ImageDataSource
    {
        //Returns all rows
        public IList<CmsImage> GetImages()
        {
            return GetImages(0, 5000);
        }

        public IList<CmsImage> GetThemeImages(int startRowIndex, int maximumRows)
        {
            int start = startRowIndex;
            int end = start + maximumRows;

            end = end - 1; //0-based

            CmsImageDao dao = new CmsImageDao();
            IList<CmsImage> images = dao.FindBySiteWithPaging(CurrentSite.Guid, start, end);

            String themeId = WebRequestContext.Instance.Request.QueryString["tid"];

            IList<CmsImage> results = new List<CmsImage>();
            foreach (CmsImage image in images)
            {
                if (image.Directory != null)
                {
                    if (image.Directory.Equals(themeId))
                        results.Add(image);
                }
            }

            return results;
        }

        public int GetThemeImageCount()
        {
            return GetThemeImages(0, 5000).Count;
        }

        public IList<CmsImage> GetImages(int startRowIndex, int maximumRows)
        {
            int start = startRowIndex;
            int end = start + maximumRows;
            
            end = end - 1; //0-based

            CmsImageDao dao = new CmsImageDao();
            return dao.FindBySiteWithPaging(CurrentSite.Guid, start, end);
        }

        public int GetImageCount()
        {
            CmsImageDao dao = new CmsImageDao();
            return dao.FindImageCount(CurrentSite.Guid);
        }
    }
}
