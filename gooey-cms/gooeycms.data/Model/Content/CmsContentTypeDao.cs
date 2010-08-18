using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Content
{
    public class CmsContentTypeDao : BaseDao
    {
        public CmsContentType FindBySiteAndName(string siteGuid, string typeName)
        {
            String hql = "select item from CmsContentType item where item.SubscriptionId = :guid and item.Name = :name";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid).SetString("name", typeName).UniqueResult<CmsContentType>();
        }

        public IList<CmsContentType> FindBySite(Guid siteGuid)
        {
            String hql = "select items from CmsContentType items where items.SubscriptionId = :guid";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsContentType>();
        }

        public CmsContentType FindByGuid(Guid guid)
        {
            String hql = "select item from CmsContentType item where item.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).UniqueResult<CmsContentType>();
        }

        public IList<CmsContentTypeField> FindFieldsByContentTypeGuid(Guid contentTypeGuid)
        {
            String hql = "select fields from CmsContentTypeField fields where fields.Parent.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", contentTypeGuid.Value).List<CmsContentTypeField>();
        }

        public IList<CmsContentType> FindGlobalContentTypes()
        {
            String hql = "select items from CmsContentType items where items.IsGlobalType = 1";
            return base.NewHqlQuery(hql).List<CmsContentType>();
        }

        public CmsContentTypeField FindFieldByContentTypeAndKey(Guid contentTypeGuid, int fieldKey)
        {
            String hql = "select field from CmsContentTypeField field where field.Parent.Guid = :guid and field.Id = :fieldKey";
            return base.NewHqlQuery(hql).SetString("guid", contentTypeGuid.Value).SetInt32("fieldKey",fieldKey).UniqueResult<CmsContentTypeField>();
        }
    }
}
