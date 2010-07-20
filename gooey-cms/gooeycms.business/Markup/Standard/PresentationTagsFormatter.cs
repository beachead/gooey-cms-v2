using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Markup;
using System.Text.RegularExpressions;

namespace Beachead.Core.Markup.Standard   
{
    public class PresentationTagsFormatter : BaseFormatter
    {
        private static Regex Comments = new Regex(@"<!--(.*?)-->", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex Bold = new Regex(@"\[b\](.*?)\[/b\]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex H4 = new Regex(@"=====(\((\w+?)\))?\s*(.*?)=====", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex H3 = new Regex(@"====(\((\w+?)\))?\s*(.*?)====", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex H2 = new Regex(@"===(\((\w+?)\))?\s*(.*?)===", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex H1 = new Regex(@"==(\((\w+?)\))?\s*(.*?)==", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex BR = new Regex(@"{br}", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex Underline = new Regex("__(.*?)__", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex Hyperlink = new Regex(@"\[(https?://.*?)(\s+(\w+))?\]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public override StringBuilder Convert(StringBuilder markup)
        {
            markup = new StringBuilder(Comments.Replace(markup.ToString(), ""));
            markup = new StringBuilder(BR.Replace(markup.ToString(), @"<br />"));
            markup = new StringBuilder(H4.Replace(markup.ToString(),@"<h4 class=""$2"">$3</h4>"));
            markup = new StringBuilder(H3.Replace(markup.ToString(), @"<h3 class=""$2"">$3</h3>"));
            markup = new StringBuilder(H2.Replace(markup.ToString(), @"<h2 class=""$2"">$3</h2>"));
            markup = new StringBuilder(H1.Replace(markup.ToString(), @"<h1 class=""$2"">$3</h1>"));
            markup = new StringBuilder(Bold.Replace(markup.ToString(), "<b>$1</b>"));

            Match match = Hyperlink.Match(markup.ToString());
            while (match.Success)
            {
                String url = match.Groups[1].Value;
                String text = match.Groups[2].Value;
                if (String.IsNullOrEmpty(text))
                    text = url;
                String formatted = String.Format(@"<a href=""{0}"">{1}</a>",url.Trim(),text.Trim());
                markup = new StringBuilder(Hyperlink.Replace(markup.ToString(), formatted, 1));

                match = match.NextMatch();
            }

            return markup;
        }
    }
}
