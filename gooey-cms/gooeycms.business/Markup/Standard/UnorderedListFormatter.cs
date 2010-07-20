using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Beachead.Core.Markup.Standard
{
    public class UnorderedListFormatter : BaseFormatter
    {
        private static Regex ListTag = new Regex(@"---+Start\s?List---+(?:\r\n)?(.*?)\n*---+End\s?List---+", RegexOptions.Compiled | RegexOptions.Singleline);

        public override StringBuilder Convert(StringBuilder markup)
        {
            StringBuilder html = new StringBuilder();

            Match match = ListTag.Match(markup.ToString());
            while (match.Success)
            {
                String items = match.Groups[1].Value;
                IList<String> results = GetListItems(items);

                html.AppendLine("<ul>");
                foreach (String result in results)
                {
                    html.AppendFormat("<li>{0}</li>", result).AppendLine();
                }
                html.AppendLine("</ul>");

                markup = new StringBuilder(ListTag.Replace(markup.ToString(), html.ToString(), 1));
                match = match.NextMatch();

                html = new StringBuilder();
            }

            return markup;
        }

        public IList<String> GetListItems(String temp)
        {
            IList<String> items = new List<String>();

            String[] matches = temp.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String match in matches)
            {
                if (!match.Trim().Equals(""))
                {
                    items.Add(base.FormatEngine.Convert(match));
                }
            }
            return items;
        }
    }
}
