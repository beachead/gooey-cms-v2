using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Core.Markup.Forms.Fields
{
    public class Textbox : BaseFormField
    {
        private const String TextboxFormat = @"<input type=""text"" name=""{0}"" id=""{0}"" class=""form-textbox persistable {1}"" />";

        /// <summary>
        /// Formats the textbox and returns the valid HTML
        /// </summary>
        /// <returns></returns>
        public override String Format()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}<br />", base.Description).AppendLine();
            builder.AppendFormat(TextboxFormat, base.Id, base.CssRequired);

            return builder.ToString();
        }
    }
}
