using System;
using System.Text;
using System.Text.RegularExpressions;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;

namespace Beachead.Core.Markup.Standard
{
    /// <summary>
    /// The header image formatter is slightly different than all of the other markup formatters.
    /// This formater will look for a string {content.header} and replace that text with
    /// the header image content.
    /// 
    /// This is done so the header image can move to outside of the content page and into the template.
    /// </summary>
    public class HeaderImageFormatter : BaseFormatter
    {
        private static Regex MarkupRegex = new Regex(@"{header-image:(?<path>.*?)\|(?<title>.*?)\|(?<caption>.*?)\|(?<color>.*?)}\r?\n?", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private String HtmlTag = CurrentSite.Configuration.HeaderImageTemplate;
        public override StringBuilder Convert(StringBuilder markup)
        {
            Match match = MarkupRegex.Match(markup.ToString());
            if (match.Success)
            {
                String result = MarkupRegex.Replace(markup.ToString(), HtmlTag);
                int startpos = result.IndexOf("<!--HEADERIMAGE-->");
                int endpos = result.IndexOf("<!--ENDHEADERIMAGE-->") + "<!--ENDHEADERIMAGE-->".Length;
                String block = result.Substring(startpos, endpos - startpos);
                
                //Remove the original header location
                result = result.Replace(block, "");

                //Move it to the correct location and update the string builder with the new markup
                result = result.Replace("{content.header}", block);
                markup = new StringBuilder(result);
            }

            return markup;
        }
    }
}
