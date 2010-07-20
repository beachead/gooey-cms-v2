using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup.Forms.Fields
{
    public class GenericText : BaseFormField
    {
        private IMarkupEngine engine;
        public GenericText(IMarkupEngine engine)
        {
            this.engine = engine;
        }

        public override string Format()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.Description);
            builder.Append(base.Markup);

            return this.engine.Convert(builder.ToString());
        }
    }
}
