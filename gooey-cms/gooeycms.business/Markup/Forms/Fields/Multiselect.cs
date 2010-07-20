using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup.Forms.Fields
{
    public class Multiselect : Dropdown
    {
        protected override string MarkupIdentifier
        {
            get { return "##"; }
        }

        protected override bool SupportsMultiple
        {
            get { return true; }
        }
    }
}
