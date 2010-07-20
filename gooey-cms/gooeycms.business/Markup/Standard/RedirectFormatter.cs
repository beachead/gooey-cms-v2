using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Beachead.Core.Markup.Standard
{
    public class RedirectFormatter : BaseFormatter
    {
        private Regex RedirectPattern = new Regex(@"\[redirect\s+(.*)\s*\]",RegexOptions.IgnoreCase);

        public override StringBuilder Convert(StringBuilder markup)
        {
            Match match = RedirectPattern.Match(markup.ToString());
            if (match.Success)
            {
                String redirectTo = match.Groups[1].Value;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", redirectTo);
                HttpContext.Current.Response.End();
            }
            return markup;
        }
    }
}
