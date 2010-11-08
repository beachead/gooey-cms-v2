using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Text.RegularExpressions;
using System.Collections;

namespace Gooeycms.Data.Model.Content
{
    public class ContentQueryBuilder : BaseDao
    {
        public class WhereInfo
        {
            public enum WhereConditional
            {
                LessThan,
                GreaterThan,
                EqualTo,
                Between
            }

            public CmsContentTypeField Field { get; set; }
            public String Value1 { get; set; }
            public String Value2 { get; set; }
            public WhereConditional Conditional { get; set; }

            public void ParseConditional(String conditional)
            {
                if (">".Equals(conditional))
                    this.Conditional = WhereConditional.GreaterThan;
                else if ("<".Equals(conditional))
                    this.Conditional = WhereConditional.LessThan;
                else if ("=".Equals(conditional))
                    this.Conditional = WhereConditional.EqualTo;
                else if ("betweet".Equals(conditional, StringComparison.InvariantCultureIgnoreCase))
                    this.Conditional = WhereConditional.Between;
                else
                    throw new ArgumentException("The conditional expression " + conditional + " is not supported.");
            }
        }

        private static Regex ConditionalMatch = new Regex(@"or|and", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex StatementMatch = new Regex(@"\(?(?<fieldname>\w+)\s*(?<condition><|>|=)\s*(?<value>[^\s]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private String subscriptionGuid = null;
        private String orderBy = null;
        private Boolean orderAscending = true;
        private String contentType;
        private Int32 limit = 0;
        private IList<WhereInfo> whereClauses = new List<WhereInfo>();
        private Boolean approvedOnly = false;

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

        public ContentQueryBuilder SetApprovedOnly(bool approvedOnly)
        {
            this.approvedOnly = approvedOnly;
            return this;
        }
        public ContentQueryBuilder SetWhereClause(String clause)
        {
            if (this.subscriptionGuid == null)
                throw new ApplicationException("The subscription guid must be set prior to calling this method");

            if (!String.IsNullOrEmpty(clause))
            {
                CmsContentTypeDao dao = new CmsContentTypeDao();

                //Get the fields and their types for the content type
                CmsContentType type = dao.FindBySiteAndName(this.subscriptionGuid, this.contentType);
                IList<CmsContentTypeField> fields = dao.FindFieldsByContentTypeGuid(type.Guid);

                //Perform error checking and make sure that there's not more than one paren pair
                int leftParens = clause.Count(f => f == '(');
                int rightParens = clause.Count(f => f == ')');

                if (leftParens > 1)
                    throw new ArgumentException("The where clause '" + clause + "' is not currently supported. Only a single set of parens may be present");
                if (leftParens != rightParens)
                    throw new ArgumentException("The where clause '" + clause + "' is not valid. Parentheses mitmatch. Expected " + leftParens + " but found " + rightParens);

                //Make sure there are not any conditional statements
                if (ConditionalMatch.IsMatch(clause))
                    throw new ArgumentException("The where clause '" + clause + "' is not valid: GooeyCms does not currently support conditional where clauses");

                //Parse the actual string
                clause = clause.Replace("'", "");
                clause = clause.Replace("(", "");
                clause = clause.Replace(")", "");

                Match match = StatementMatch.Match(clause);
                if (!match.Success)
                    throw new ArgumentException("The where clause '" + clause + "' is not in a valid format. Format should be: field-name [<,>,=] value. Greater-than and Less-than are only supported on datetime field types.");

                String fieldname = match.Groups["fieldname"].Value;
                String condition = match.Groups["condition"].Value;
                String value = match.Groups["value"].Value;

                //Make sure that this fieldname is valid for the content type
                CmsContentTypeField field = null;
                try
                {
                    field = fields.Single(f => f.SystemName.Equals(fieldname));
                }
                catch (InvalidOperationException e)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (CmsContentTypeField temp in fields)
                    {
                        builder.Append(temp.SystemName + "|");
                    }
                    throw new ArgumentException("The where clause '" + clause + "' is not valid: The content type '" + type.Name + "' does not contain the field '" + fieldname + "'. Available fields:" + builder.ToString());
                }

                WhereInfo whereInfo = new WhereInfo();
                whereInfo.Value1 = value;
                whereInfo.Field = field;
                whereInfo.ParseConditional(condition);

                this.whereClauses.Add(whereInfo);
            }
            return this;
        }


        public IList<CmsContent> ExecuteQuery()
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("select distinct content.Id from CmsContent content join content._Fields fields where content.SubscriptionId = :subscriptionGuid ");
            hql.Append("and content.ContentType.Name = :contentTypeName ");

            if (this.approvedOnly)
            {
                hql.Append(" and content.IsApproved = 1 ");
            }

            //Check if we have a where clause which needs to be added
            if (this.whereClauses.Count > 0)
            {
                for (int i = 0; i < this.whereClauses.Count; i++)
                {
                    WhereInfo where = this.whereClauses[i];

                    hql.Append("and fields.Name = :fieldname" + i + " and ");
                    switch (where.Field.ObjectType)
                    {
                        case "System.DateTime":
                            hql.Append("convert(datetime,fields.Value) ");
                            break;
                        case "System.Boolean":
                            hql.Append("convert(bit,fields.Value) ");
                            break;
                        default: //System.String
                            hql.Append("fields.Value ");
                            break;
                    }

                    switch (where.Conditional)
                    {
                        case WhereInfo.WhereConditional.EqualTo:
                            hql.Append(" = ");
                            break;
                        case WhereInfo.WhereConditional.LessThan:
                            hql.Append(" < ");
                            break;
                        case WhereInfo.WhereConditional.GreaterThan:
                            hql.Append(" > ");
                            break;
                        case WhereInfo.WhereConditional.Between:
                            hql.Append(" between (");
                            break;
                        default:
                            throw new ApplicationException("The supplied conditional is not currently supported");
                    }

                    hql.Append(":value1_" + i);
                    if (where.Value2 != null)
                        hql.Append(" and ").Append(":value2_" + i).Append(")");
                }
            }

            IQuery query = base.NewHqlQuery(hql.ToString());
            query = query.SetString("subscriptionGuid", this.subscriptionGuid);
            query = query.SetString("contentTypeName", this.contentType);

            for (int i = 0; i < whereClauses.Count; i++)
            {
                WhereInfo where = whereClauses[i];
                query.SetString("fieldname" + i, where.Field.SystemName);

                switch (where.Field.ObjectType)
                {
                    case "System.DateTime":
                        query.SetDateTime("value1_" + i, DateTime.Parse(where.Value1));
                        break;
                    case "System.Boolean":
                        query.SetBoolean("value1_" + i, Boolean.Parse(where.Value1));
                        break;
                    default: //System.String
                        query.SetString("value1_" + i, where.Value1);
                        break;
                }
            }

            IList<Int32> temp = query.List<Int32>();
            IList<CmsContent> results = null;
            if ((temp != null) && (temp.Count > 0))
            {
                HashSet<Int32> uniquePrimaryKeys = new HashSet<Int32>(temp);

                hql = new StringBuilder();
                hql.Append("select content from CmsContent content where content.Id in (:primaryKeys)");
                results = base.NewHqlQuery(hql.ToString()).SetParameterList("primaryKeys", (IList)uniquePrimaryKeys.ToList<Int32>()).List<CmsContent>();

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

                //Check if we need to limit the results
                if (this.limit > 0)
                    results = results.Take(this.limit).ToList();
            }

            if (results == null)
                results = new List<CmsContent>();

            return results;
        }
    }
}
