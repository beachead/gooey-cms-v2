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

        private static Regex Field = new Regex(@"{(?<id>\w+)\.(?<key>\w+)}", RegexOptions.Compiled);
        public override StringBuilder Convert(StringBuilder stringBuilder)
        {
            String content = stringBuilder.ToString();
            content = Field.Replace(content, new MatchEvaluator(FieldMatchEvaluator));       

            return new StringBuilder(content);
        }

        private void loadContentFromQuerystring(String contentType)
        {
            String guid = WebRequestContext.Instance.Request[contentType];
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
