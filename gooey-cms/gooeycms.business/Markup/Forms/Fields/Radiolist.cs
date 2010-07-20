using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup.Forms.Fields
{
    public class Radiolist : BaseFormField
    {
        public override string Format()
        {
            int count = 0;
            StringBuilder builder = new StringBuilder();
            String markup = base.Markup;
            String[] items = markup.Split(new String[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

            builder.AppendFormat("{0}<br />", base.Description);
            builder.AppendLine(@"<div class=""webscript-form-radio"">");
            foreach (String item in items)
            {
                String selected = "";
                if ((count == 0) && (!base.IsRequired))
                    selected = @"checked=""checked""";

                builder.AppendFormat(@"<input type=""radio"" id=""{0}_{3}"" name=""{1}"" value=""{2}"" {4} />&nbsp;{2}<br />", base.Id, base.Id, base.HtmlEncode(item.Trim()), count++, selected);
                builder.AppendLine("");
            }
            builder.AppendLine(@"</div>");

            return builder.ToString();
        }
    }
}
