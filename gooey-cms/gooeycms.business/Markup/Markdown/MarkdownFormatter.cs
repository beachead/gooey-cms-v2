using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup;
using System.Text.RegularExpressions;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Markup.Markdown
{
    public class MarkdownFormatter : BaseFormatter
    {
        private static Regex ImageHtml = new Regex(@"<img\s+.*?src\=([\x27\x22])?(?<Url>[^\x27\x22|\s|\\|>]*)(?=[\x27\x22])?.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex ImageInputHtml = new Regex(@"<input\s+.*?src\=([\x27\x22])?(?<Url>[^\x27\x22|\s|\\|>]*)(?=[\x27\x22])?.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex FlashHtml = new Regex(@"~/(\w+)\.swf", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private MarkdownSharp.Markdown formatter = new MarkdownSharp.Markdown();
        private String imageContainerUrl;

        public override StringBuilder Convert(StringBuilder markup)
        {
            imageContainerUrl = CurrentSite.GetContainerUrl(SiteHelper.ImagesContainerKey);
            if (base.IsPartOfTheme)
            {
                String guid = CurrentSite.GetCurrentTheme().ThemeGuid;
                imageContainerUrl = imageContainerUrl + "/" + guid;
            }

            formatter.AutoHyperlink = true;
            String html = formatter.Transform(markup.ToString());

            //rewrite all <img> urls to the container
            html = ImageHtml.Replace(html,new MatchEvaluator(ImageReferenceEvaluator));
            html = ImageInputHtml.Replace(html, new MatchEvaluator(ImageReferenceEvaluator));

            //rewrite all of the flash urls to the container
            html = FlashHtml.Replace(html, new MatchEvaluator(FlashReferenceEvaluator));

            return new StringBuilder(html);
        }

        private string FlashReferenceEvaluator(Match match)
        {
            String fullurl = match.Groups[0].Value;

            return fullurl.Replace("~", imageContainerUrl);
        }

        private string ImageReferenceEvaluator(Match match)
        {
            String origurl = match.Groups["Url"].Value;
            String fullimagetag = match.Groups[0].Value;

            String filename = origurl;
            int pos = origurl.LastIndexOf("/");
            if (pos >= 0)
                filename = origurl.Substring(pos + 1);
            String imagetag = fullimagetag.Replace(origurl, imageContainerUrl + "/" + filename);

            return imagetag;
        }
    }
}
