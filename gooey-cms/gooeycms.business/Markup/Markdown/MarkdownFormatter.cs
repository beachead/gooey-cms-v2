using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup;
using System.Text.RegularExpressions;
using Gooeycms.Business.Util;
using Gooeycms.Business.Images;

namespace Gooeycms.Business.Markup.Markdown
{
    public class MarkdownFormatter : BaseFormatter
    {
        private MarkdownSharp.Markdown formatter = new MarkdownSharp.Markdown();

        public override StringBuilder Convert(StringBuilder markup)
        {
            formatter.AutoHyperlink = true;
            String html = formatter.Transform(markup.ToString());

            if (this.IsPartOfTheme)
                html = ImageRewriter.ThemesImageRewriter.Rewrite(html);
            else
                html = ImageRewriter.PageImageRewriter.Rewrite(html);

            return new StringBuilder(html);
        }

 
    }
}
