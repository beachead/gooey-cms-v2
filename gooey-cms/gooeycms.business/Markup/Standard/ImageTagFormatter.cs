using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Beachead.Core.Markup.Standard
{
    public class ImageTagFormatter : BaseFormatter
    {
        private static Regex Image = new Regex(@"\[\[image:\s*(.*?)(\|(.*?))?\]\]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        #region IMarkupFormatter Members

        public override StringBuilder Convert(StringBuilder markup)
        {
            markup = new StringBuilder(Image.Replace(markup.ToString(),@"<img src=""$1"" alt=""$3"" />"));

            return markup;
        }

        #endregion
    }
}
