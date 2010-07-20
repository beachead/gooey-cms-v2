using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup.Forms.Fields
{
    public class Dropdown : BaseFormField
    {
        protected virtual String MarkupIdentifier
        {
            get { return "+"; }
        }

        public override string Format()
        {
            StringBuilder builder = new StringBuilder();

            String markup = base.Markup;
            String[] items = markup.Split(new String[] { MarkupIdentifier }, StringSplitOptions.RemoveEmptyEntries);
            String markupAttributes = "";

            if (SupportsMultiple)
            {
                markupAttributes = String.Format(@"class=""form-multiple {0} persistable"" multiple=""multiple""", base.CssRequired);
            }
            else
            {
                markupAttributes = String.Format(@"class=""form-combo {0}""", base.CssRequired);
            }

            builder.AppendFormat("{0}<br />", base.Description);
            builder.AppendFormat(@"<select id=""{0}"" name=""{1}"" {2}>", base.Id, base.Id, markupAttributes);
            if ((base.IsRequired) && (!SupportsMultiple))
                builder.Append(@"<option value="""">&nbsp;</option>");

            foreach (String item in items)
                builder.AppendFormat(@"<option value=""{0}"">{0}</option>", base.HtmlEncode(item.Trim()));

            builder.AppendFormat(@"</select>");

            return builder.ToString();
        }

        /// <summary>
        /// Value which will determine whether this drop-down supports choosing multiples
        /// </summary>
        /// <returns></returns>
        protected virtual bool SupportsMultiple
        {
            get { return false; }
        }
    }
}
