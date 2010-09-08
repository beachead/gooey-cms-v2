using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup;
using Gooeycms.Business.Markup.Markdown;

namespace Gooeycms.Business.Markup.Engine
{
    public class MarkdownMarkupEngine : MarkupEngine
    {
        public override IList<IMarkupFormatter> GetFormatters()
        {
            IList<IMarkupFormatter> formatters = new List<IMarkupFormatter>();
            formatters.Add(new MarkdownFormatter());

            return formatters;
        }

        public override string ConvertToHtml(IList<IMarkupFormatter> formatters, string markup, bool isPartOfTheme)
        {
            StringBuilder builder = new StringBuilder(markup);
            foreach (IMarkupFormatter formatter in formatters)
            {
                formatter.FormatEngine = this;
                formatter.IsPartOfTheme = isPartOfTheme;
                builder = formatter.Convert(builder);
            }

            return builder.ToString();
        }
    }
}
