using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using System.Text.RegularExpressions;

namespace Gooeycms.Business.Images
{
    public class ImageRewriter
    {
        private static Regex ImageHtml = new Regex(@"<img\s+.*?src\=([\x27\x22])?(?<Url>[^\x27\x22|\s|\\|>]*)(?=[\x27\x22])?.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex ImageInputHtml = new Regex(@"<input\s+.*?src\=([\x27\x22])?(?<Url>[^\x27\x22|\s|\\|>]*)(?=[\x27\x22])?.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex FlashHtml = new Regex(@"~/(\w+)\.swf", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public enum ImageContainerLocation
        {
            Page,
            Theme
        }

        public static ImageRewriter ThemesImageRewriter
        {
            get
            {
                return new ImageRewriter(ImageContainerLocation.Theme);
            }
        }
        public static ImageRewriter PageImageRewriter
        {
            get
            {
                return new ImageRewriter(ImageContainerLocation.Page);
            }
        }

        private String imageContainerUrl;
        public ImageRewriter(ImageContainerLocation location)
        {
            imageContainerUrl = CurrentSite.GetContainerUrl(SiteHelper.ImagesContainerKey);
            if (location == ImageContainerLocation.Theme)
            {
                String guid = CurrentSite.GetCurrentTheme().ThemeGuid;
                imageContainerUrl = imageContainerUrl + "/" + guid;
            }
        }

        public ImageRewriter(String containerLocation)
        {
            this.imageContainerUrl = containerLocation;
        }

        public String Rewrite(String html)
        {
            html = ImageHtml.Replace(html, new MatchEvaluator(ImageReferenceEvaluator));
            html = ImageInputHtml.Replace(html, new MatchEvaluator(ImageReferenceEvaluator));
            //html = FlashHtml.Replace(html, new MatchEvaluator(FlashReferenceEvaluator)); //performed in a stand-alone handler

            return html;
        }

        public StringBuilder Rewrite(StringBuilder builder)
        {
            String html = builder.ToString();
            html = Rewrite(html);

            builder.Clear();
            builder = new StringBuilder(html);

            return builder;
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

        private string FlashReferenceEvaluator(Match match)
        {
            /* do not rewrite flash (at this point)
            String fullurl = match.Groups[0].Value;

            return fullurl.Replace("~", imageContainerUrl);
            */

            return match.Groups[0].Value;
        }  
    }
}
