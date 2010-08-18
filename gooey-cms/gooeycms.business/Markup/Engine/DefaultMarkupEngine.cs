using System.Collections.Generic;
using System.Text;
using Beachead.Core.Markup.Forms;
using Beachead.Core.Markup.Standard;
using System;

namespace Beachead.Core.Markup.Engine
{
    /// <summary>
    /// Loads the markup plugins from the configuration file.
    /// </summary>
    public class DefaultMarkupEngine : MarkupEngine
    {
        #region IMarkupEngine Members

        public override IList<IMarkupFormatter> GetFormatters()
        {
            IList<IMarkupFormatter> formatters = new List<IMarkupFormatter>();
            formatters.Add(new RedirectFormatter());
            //formatters.Add(new DynamicContentFormatter());
            formatters.Add(new PresentationTagsFormatter());
            formatters.Add(new ImageTagFormatter());
            formatters.Add(new TableTagFormatter());
            formatters.Add(new UnorderedListFormatter());
            formatters.Add(new FormMarkupFormatter());

            return formatters;
        }

        public override string ConvertToHtml(IList<IMarkupFormatter> formatters, string markup, Boolean isPartOfTheme)
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

        #endregion
    }
}
