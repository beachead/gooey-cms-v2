using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup.Forms.Fields
{
    public interface IFormField
    {
        String Id { get; set; }
        String Markup { get; set; }
        String Description { get; set; }
        Boolean IsRequired { get; set; }
        Boolean IsHidden { get; set; }
        String Format();
    }
}
