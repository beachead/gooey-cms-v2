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

            String themeId = WebRequestContext.Instance.Request.QueryString["tid"];

            CmsImageDao dao = new CmsImageDao();
            return dao.FindBySiteWithPagingAndDirectory(CurrentSite.Guid, themeId, start, end);
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
