using System.Text;
using Beachead.Core.Markup;
using System.Text.RegularExpressions;
using System;
using Gooeycms.Data.Model.Content;
using System.Collections.Generic;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;
using Gooeycms.Extensions;
namespace Gooeycms.Business.Markup.Dynamic
{
    class DynamicContentFormatter : BaseFormatter
    {
        private CmsContent loadedContent = null;
        private String loadedContentType = null;

        private static Regex ForEachBlock = new Regex(@"<%\s*foreach\s+(?<id>\w+)\s*(where(?<where>.*?))?\s*(orderby(?<orderby>.*?))?\s*(limit(?<limit>.*?))?\s*%>(?<block>.*?)<%\s*next\s*%>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static Regex Field = new Regex(@"{(?<id>\w+)\.(?<key>\w+)}", RegexOptions.Compiled);
        private static Regex OrderBy = new Regex(@"(?<field>\w+)\s*(?<direction>asc|desc)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex Limit = new Regex(@"\d+", RegexOptions.Compiled);

        public override StringBuilder Convert(StringBuilder stringBuilder)
        {
            String content = stringBuilder.ToString();

            //Find any blocks of dynamic code
            content = ForEachBlock.Replace(content, new MatchEvaluator(BlockMatchEvaluator));

            //Find non-block dynamic content strings (must find a matching type in the querystring which specifies the guid of the content to load)
            content = Field.Replace(content, new MatchEvaluator(FieldMatchEvaluator));       

            return new StringBuilder(content);
        }

        private void loadContentFromQuerystring(String contentType)
        {
            String guid = WebRequestContext.Instance.Request[contentType];
            
            //If the guid is null, check if they used the default identifier instead of a content type identifier
            if (guid == null)
                guid = WebRequestContext.Instance.Request.QueryString["cid"];

            if (guid != null)
            {
                CmsContentDao dao = new CmsContentDao();
                loadedContent = dao.FindByGuid(CurrentSite.Guid, guid);
                loadedContentType = contentType;
            }
        }

        private Boolean Validate(String contentType)
        {
            return (contentType.EqualsCaseInsensitive(loadedContentType));
        }

        private string BlockMatchEvaluator(Match match)
        {
            String contentType = match.Groups["id"].Value;
            String orderBy = match.Groups["orderby"].Value;
            String limitBy = match.Groups["limit"].Value;
            String block = match.Groups["block"].Value;
            String where = match.Groups["where"].Value;

            String orderByField = null;
            String orderByDirection = "asc";
            Match orderByMatch = OrderBy.Match(orderBy);
            if (orderByMatch.Success)
            {
                orderByField = orderByMatch.Groups["field"].Value;
                orderByDirection = orderByMatch.Groups["direction"].Value;
            }

            int limit = 0;
            Match limitMatch = Limit.Match(limitBy);
            if (limitMatch.Success)
            {
                limit = Int32.Parse(limitMatch.Value);
            }

            //Perform the search for the content type
            ContentQueryBuilder query = new ContentQueryBuilder();
            IList<CmsContent> results = query.SetSubscriptionGuid(CurrentSite.Guid)
                                             .SetContentType(contentType)
                                             .SetOrderBy(orderByField, orderByDirection)
                                             .SetLimit(limit)
                                             .SetWhereClause(where)
                                             .ExecuteQuery();

            StringBuilder replacement = new StringBuilder();

            this.loadedContentType = contentType;
            foreach (CmsContent item in results)
            {
                this.loadedContent = item;
                String result = Field.Replace(block, new MatchEvaluator(FieldMatchEvaluator));

                replacement.Append(result);
            }
            this.loadedContentType = null;
            this.loadedContent = null;

            return replacement.ToString();
        }

        private string FieldMatchEvaluator(Match match)
        {
            String contentType = match.Groups["id"].Value;
            String fieldName = match.Groups["key"].Value;

            if (loadedContentType == null)
                loadContentFromQuerystring(contentType);
            
            //make sure the content types are the same
            String result = match.Value;
            if (Validate(contentType))
            {
                CmsContentField field = this.loadedContent.FindField(fieldName);
                if (field != null)
                    result = field.Value;
            }

            return result;
        }
    }
}
