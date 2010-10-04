using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup;
using Gooeycms.Business.Markup.Markdown;
using Gooeycms.Business.Markup.Forms_v2;
using Beachead.Core.Markup.Standard;
using Gooeycms.Business.Markup.Dynamic;

namespace Gooeycms.Business.Markup.Engine
{
    public class MarkdownMarkupEngine : MarkupEngine
    {
        public override IList<IMarkupFormatter> GetFormatters()
        {
            IList<IMarkupFormatter> formatters = new List<IMarkupFormatter>();
            formatters.Add(new DynamicContentFormatter());
            formatters.Add(new TableTagFormatter());
            formatters.Add(new MarkdownFormatter());
            formatters.Add(new FormMarkupFormatter());

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
