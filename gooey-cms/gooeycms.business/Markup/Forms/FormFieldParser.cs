using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Core.Markup.Standard;
using Beachead.Core.Markup.Forms.Fields;


namespace Beachead.Core.Markup.Forms
{
    public class FormFieldParser
    {
        String formId;
        IMarkupEngine engine;
        public FormFieldParser(String id, IMarkupEngine engine)
        {
            this.formId = id;
            this.engine = engine;
        }

        /// <summary>
        /// Converts the markup into valid XHTML form fields.
        /// 
        /// Requires the TableTagFormatter to parse the table structure.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public StringBuilder Convert(StringBuilder html, String content)
        {
            TableTagFormatter table = new TableTagFormatter();
            IList<TableTagFormatter.TableRow> rows = table.Parse(content);
            html.Append(@"<table>").AppendLine();
            foreach (TableTagFormatter.TableRow row in rows)
            {
                html.Append("  <tr>").AppendLine();
                foreach (TableTagFormatter.TableCell cell in row.TableCells)
                {
                    html.AppendFormat("    <td colspan={0}>\r\n",cell.Colspan);
                    
                    String formatted = null;
                    IFormField field = this.GetFormField(cell.Value);
                    if (field != null)
                        formatted = field.Format();
                    else
                        formatted = "!!ERROR IN FORM MARKUP!!";

                    html.Append(formatted).AppendLine();
                    html.AppendLine("    </td>");
                }
                html.Append("  </tr>").AppendLine();
            }
            html.Append(@"</table>").AppendLine();

            return html;
        }

        /// <summary>
        /// Determines the appropriate form field class to use and then
        /// returns that to the caller.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public IFormField GetFormField(String content)
        {
            IFormField field = null;
            String[] lines = content.Split(new String [] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder markup = new StringBuilder();
            for (int i = 2; i < lines.Length; i++)
                markup.AppendLine(lines[i].Trim());

            //There will always be at least two lines, otherwise, the markup is invalid
            if (lines.Length >= 2)
            {
                String id = lines[0].Trim();
                String description = lines[1].Trim();
                
                //Determine if the field is required or not
                bool required = id.Contains("*");
                bool hidden = id.Contains("hidden");
                id = id.Replace("hidden", "");

                if (required)
                {
                    id = id.Replace("*", "");
                    description = description + " *";
                }
                id = id.Trim();

                if (id.Equals("::markup"))
                {
                    field = new GenericText(this.engine);
                }
                else
                {
                    if (lines.Length == 2)
                    {
                        if (lines[1].StartsWith("^"))
                            field = new Checkbox();
                        else
                            field = new Textbox();
                    }
                    else
                    {
                        if (lines[2].StartsWith("("))
                            field = new Textarea();
                        else if (lines[2].StartsWith("^"))
                            field = new Checkbox();
                        else if (lines[2].StartsWith("+"))
                            field = new Dropdown();
                        else if (lines[2].StartsWith("##"))
                            field = new Multiselect();
                        else if (lines[2].StartsWith("#"))
                            field = new Radiolist();
                    }
                }

                if (field != null)
                {
                    field.Id = String.Format("{0}_{1}", this.formId, id);
                    field.IsRequired = required;
                    field.IsHidden = hidden;
                    field.Description = description.Trim();
                    field.Markup = markup.ToString().Trim();
                }
            }

            return field;
        }
    }
}
