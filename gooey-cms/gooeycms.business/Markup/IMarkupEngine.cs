using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup
{
    public interface IMarkupEngine
    {
        /// <summary>
        /// Public interface that clients will call to perform the conversion
        /// </summary>
        /// <param name="markup"></param>
        /// <returns></returns>
        String Convert(String markup);
        String Convert(String markup, Boolean isPartofTheme);

        /// <summary>
        /// Retrieves the header formatter used by the theme
        /// </summary>
        /// <returns></returns>
        IMarkupFormatter GetHeaderFormatter();

        IMarkupFormatter GetHeaderTextFormatter();
    }
}
