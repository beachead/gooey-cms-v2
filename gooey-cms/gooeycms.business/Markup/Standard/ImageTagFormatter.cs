using System;
using System.Text;
using System.Text.RegularExpressions;
using Gooeycms.Business.Util;

namespace Beachead.Core.Markup.Standard
{
    public class ImageTagFormatter : BaseFormatter
    {
        private static Regex Image = new Regex(@"\[\[image:\s*~(.*?)(\|(.*?))?\s*(class=(.*?))?\]\]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex ImageHtml = new Regex(@"<img\s+.*?src\=([\x27\x22])?(?<Url>[^\x27\x22|\s|\\|>]*)(?=[\x27\x22])?.*?>",RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #region IMarkupFormatter Members

        public override StringBuilder Convert(StringBuilder markup)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.ImagesDirectoryKey);
            if (base.IsPartOfTheme)
            {
                String guid = CurrentSite.GetCurrentTheme().ThemeGuid;
                container = container + "/" + guid;
            }

            //rewrite all <img> urls to the container
            Match match = ImageHtml.Match(markup.ToString());
            while (match.Success)
            {
                String origurl = match.Groups["Url"].Value;
                String fullimagetag = match.Groups[0].Value;
                
                String filename = origurl;
                int pos = origurl.LastIndexOf("/");
                if (pos >= 0)
                    filename = origurl.Substring(pos + 1);
                String imagetag = fullimagetag.Replace(origurl, container + "/" + filename);

                markup = new StringBuilder(markup.ToString().Replace(fullimagetag,imagetag));
                match = match.NextMatch();
            }

            markup = new StringBuilder(Image.Replace(markup.ToString(),@"<img src=""" + container + @"$1"" alt=""$3"" class=""$5"" />"));

            return markup;
        }

        #endregion
    }
}
