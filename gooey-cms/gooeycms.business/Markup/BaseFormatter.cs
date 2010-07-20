using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup
{
    /// <summary>
    /// Base class that the formatters can implement to gain access to
    /// common functionality.
    /// </summary>
    public abstract class BaseFormatter : IMarkupFormatter
    {
        #region IMarkupFormatter Members
        private IMarkupEngine formatter;

        /// <summary>
        /// Provides access to the format engine which allows the individual
        /// formatters to also format their inner markup content
        /// </summary>
        public IMarkupEngine FormatEngine
        {
            get { return this.formatter; }
            set { this.formatter = value; }
        }

        public abstract StringBuilder Convert(StringBuilder markup);

        #endregion
    }
}
