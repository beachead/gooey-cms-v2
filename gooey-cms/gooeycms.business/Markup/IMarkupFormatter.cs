using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup
{
    public interface IMarkupFormatter
    {
        IMarkupEngine FormatEngine { get; set; }
        Boolean IsPartOfTheme { get; set; }
        StringBuilder Convert(StringBuilder markup);
    }
}
