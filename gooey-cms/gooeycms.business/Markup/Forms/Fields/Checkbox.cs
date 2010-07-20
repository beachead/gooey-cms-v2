using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup.Forms.Fields
{
    public class Checkbox : BaseFormField
    {
        public override string Format()
        {
            int count = 0;
            StringBuilder builder = new StringBuilder();
            String markup;
            bool hasDescription = false;
            if (base.Description.StartsWith("^"))
                markup = base.Description; //Use the description, since this markup only has two lines
            else
            {
                hasDescription = true;
                markup = base.Markup;
            }

            String[] items = markup.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
            builder.AppendFormat(@"<div class=""form-checkbox"" style=""{0}"">",base.CssHidden).AppendLine();

            if (hasDescription)
                builder.AppendFormat("{0}<br />", base.Description);

            String value = "yes";
            foreach (String item in items)
            {
                if (hasDescription)
                    value = base.HtmlEncode(item.Trim());
                String isChecked = "";
                if (base.IsHidden)
                    isChecked = @"checked=""true""";

                builder.AppendFormat(@"<input type=""checkbox"" id=""{0}_{1}"" name=""{2}"" class=""{3}"" style=""{6}"" value=""{5}"" {7}/>&nbsp;{4}<br />", base.Id, count++, base.Id, base.CssRequired, base.HtmlEncode((item.Trim())), value,base.CssHidden, isChecked);
                builder.AppendLine();
            }
            builder.AppendLine("</div>");

            return builder.ToString();
        }
    }
}
