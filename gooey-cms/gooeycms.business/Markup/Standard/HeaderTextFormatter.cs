using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Beachead.Core.Markup.Standard
{
    public class HeaderTextFormatter : BaseFormatter
    {
        private static Regex HeaderTextPattern = new Regex(@"{header-text\s+(.*?)}", RegexOptions.IgnoreCase);
        public override StringBuilder Convert(StringBuilder markup)
        {
            Match match = HeaderTextPattern.Match(markup.ToString());
            if (match.Success)
            {
                String header = match.Groups[1].Value.Trim();
                markup = new StringBuilder(HeaderTextPattern.Replace(markup.ToString(), ""));
                markup = markup.Replace("{header-text}", "<h1>" + header + "</h1>");
            }

            return markup;
        }
    }
}
