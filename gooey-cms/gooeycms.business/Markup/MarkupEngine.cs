using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup.Standard;

namespace Beachead.Core.Markup
{
    public abstract class MarkupEngine : IMarkupEngine
    {
        public abstract IList<IMarkupFormatter> GetFormatters();
        public abstract String ConvertToHtml(IList<IMarkupFormatter> formatters, String markup, Boolean isPartOfTheme);

        /// <summary>
        /// Abstract class which implements the Convert method and then calls the 
        /// two other methods to get the formatters and convert them to HTML
        /// </summary>
        /// <param name="markup"></param>
        /// <returns></returns>
        public String Convert(String markup)
        {
            return Convert(markup, false);
        }

        public String Convert(String markup, Boolean isPartOfTheme)
        {
            IList<IMarkupFormatter> formatters = this.GetFormatters();
            return this.ConvertToHtml(formatters, markup, isPartOfTheme);
        }

        public IMarkupFormatter GetHeaderFormatter()
        {
            return new HeaderImageFormatter();
        }

        public IMarkupFormatter GetHeaderTextFormatter()
        {
            return new HeaderTextFormatter();
        }
    }
}
