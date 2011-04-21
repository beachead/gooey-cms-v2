using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup;
using Gooeycms.Business.Markup.Markdown;
using Gooeycms.Business.Markup.Forms_v2;
using Beachead.Core.Markup.Standard;
using Gooeycms.Business.Markup.Dynamic;
using System.Text.RegularExpressions;

namespace Gooeycms.Business.Markup.Engine
{
    public class MarkdownMarkupEngine : MarkupEngine
    {
        private Regex NoMarkupPattern = new Regex(@"<!--\s*nomarkup-begin\s*-->(.*?)<!--\s*nomarkup-end\s*-->", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

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
            StringBuilder builder = new StringBuilder();
            if (markup != null)
            {
                //Remove any blocks which have been set to nomarkup
                MatchCollection matches = NoMarkupPattern.Matches(markup);
                IList<String> nomarkupBlocks = new List<String>();
                for (int i = 0; i < matches.Count; i++)
                {
                    Match match = matches[i];
                    nomarkupBlocks.Add(match.Groups[1].Value);
                    markup = NoMarkupPattern.Replace(markup, "{nomarkup_" + i + "}", 1);
                }

                //Format the markup
                builder = new StringBuilder(markup);
                foreach (IMarkupFormatter formatter in formatters)
                {
                    formatter.FormatEngine = this;
                    formatter.IsPartOfTheme = isPartOfTheme;
                    builder = formatter.Convert(builder);
                }

                //Restore any nomarkup blocks back to their original form
                for (int i = 0; i < nomarkupBlocks.Count; i++)
                {
                    String block = nomarkupBlocks[i];
                    builder = builder.Replace("{nomarkup_" + i + "}", block);
                }
            }
            return builder.ToString();
        }
    }
}
