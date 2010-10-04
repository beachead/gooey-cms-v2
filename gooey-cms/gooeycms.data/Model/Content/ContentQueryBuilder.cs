using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Gooeycms.Data.Model.Content
{
    public class ContentQueryBuilder : BaseDao
    {
        private String subscriptionGuid = null;
        private String orderBy = null;
        private Boolean orderAscending = true;
        private String contentType;
        private Int32 limit = 0;

        public ContentQueryBuilder SetSubscriptionGuid(Guid guid)
        {
            this.subscriptionGuid = guid.Value;
            return this;
        }

        public ContentQueryBuilder SetContentType(String value)
        {
            this.contentType = value;
            return this;
        }
        public ContentQueryBuilder SetOrderBy(String value, String direction)
        {
            this.orderBy = value;
            if ("desc".Equals(direction))
                this.orderAscending = false;

            return this;
        }
        public ContentQueryBuilder SetLimit(int value)
        {
            this.limit = value;
            return this;
        }


        public IList<CmsContent> ExecuteQuery()
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("select content from CmsContent content where content.SubscriptionId = :subscriptionGuid ");
            hql.Append("and content.ContentType.Name = :contentTypeName ");

            IQuery query = base.NewHqlQuery(hql.ToString());

            if (limit > 0)
            {
                query.SetMaxResults(limit);
            }
            query = query.SetString("subscriptionGuid", this.subscriptionGuid);
            query = query.SetString("contentTypeName", this.contentType);

            IList<CmsContent> results = query.List<CmsContent>();

            //Check if we need to sort the results
            if (this.orderBy != null)
            {
                ((List<CmsContent>)results).Sort(delegate(CmsContent left, CmsContent right)
                {
                    //Pull out the order-by field for each of the items
                    CmsContentField leftCompareField = left.FindField(this.orderBy);
                    CmsContentField rightCompareField = right.FindField(this.orderBy);
                    if (leftCompareField == null)
                        throw new ApplicationException("Invalid order-by specified in select clause. '" + this.orderBy + "' is not a valid field name");

                    IComparable leftCompare = leftCompareField.GetValueAsComparable();
                    IComparable rightCompare = rightCompareField.GetValueAsComparable();

                    return leftCompare.CompareTo(rightCompare);
                });

                if (!this.orderAscending)
                    ((List<CmsContent>)results).Reverse();
            }

            return results;
        }
    }
}
