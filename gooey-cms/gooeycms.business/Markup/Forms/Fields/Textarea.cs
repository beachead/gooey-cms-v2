using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Beachead.Core.Markup.Forms.Fields
{
    public class Textarea : BaseFormField
    {
        private const String TextAreaFormat = @"<textarea class=""form-textarea {4} persistable"" id=""{0}"" name=""{1}"" cols=""{2}"" rows=""{3}""></textarea>";
        private Regex Pattern = new Regex(@"\(\s*(\d+),\s*(\d+)\s*\)", RegexOptions.Multiline | RegexOptions.Compiled);

        public override string Format()
        {
            int cols = 35;
            int rows = 10;

            Match match = Pattern.Match(base.Markup);
            if (match.Success)
            {
                cols = Int32.Parse(match.Groups[1].Value);
                rows = Int32.Parse(match.Groups[2].Value);
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"{0}<br />",base.Description).AppendLine();
            builder.AppendFormat(TextAreaFormat, this.Id, this.Id, cols, rows, base.IsRequired);

            return builder.ToString();
        }
    }
}
