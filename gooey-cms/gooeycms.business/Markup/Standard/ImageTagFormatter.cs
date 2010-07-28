using System;
using System.Text;
using System.Text.RegularExpressions;
using Gooeycms.Business.Util;

namespace Beachead.Core.Markup.Standard
{
    public class ImageTagFormatter : BaseFormatter
    {
        private static Regex Image = new Regex(@"\[\[image:\s*~(.*?)(\|(.*?))?\]\]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        #region IMarkupFormatter Members

        public override StringBuilder Convert(StringBuilder markup)
        {
            String container = CurrentSite.GetContainerUrl(SiteHelper.ImagesDirectoryKey);
            markup = new StringBuilder(Image.Replace(markup.ToString(),@"<img src=""" + container + @"$1"" alt=""$3"" />"));

            return markup;
        }

        #endregion
    }
}
