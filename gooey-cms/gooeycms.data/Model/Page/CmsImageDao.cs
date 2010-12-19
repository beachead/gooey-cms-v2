using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Gooeycms.Data.Model.Page
{
    public class CmsImageDao : BaseDao
    {

        public CmsImage FindByUrl(string url)
        {
            String hql = "select image from CmsImage image where image.CloudUrl = :url";
            return base.NewHqlQuery(hql).SetString("url", url).UniqueResult<CmsImage>();
        }

        public IList<CmsImage> FindBySiteWithPaging(Guid siteGuid, int start, int end)
        {

            return base.Session.GetNamedQuery("CmsImagesPaging").SetParameter("guid",siteGuid.Value).SetInt32("start",start).SetInt32("end",end).List<CmsImage>();
        }

        public int FindImageCount(Guid guid)
        {
            String hql = "select count(image) from CmsImage image where image.SubscriptionId = :guid";
            long temp = base.NewHqlQuery(hql).SetString("guid", guid.Value).UniqueResult<long>();

            return Convert.ToInt32(temp);
        }

        public CmsImage FindBySiteAndGuid(Guid siteGuid, Guid guid)
        {
            String hql = "select image from CmsImage image where image.SubscriptionId = :siteGuid and image.Guid = :guid";

            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid.Value).SetString("guid", guid.Value).UniqueResult<CmsImage>();
        }
    }
}
