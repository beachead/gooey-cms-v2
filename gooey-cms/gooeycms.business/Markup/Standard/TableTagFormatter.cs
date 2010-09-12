using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Beachead.Core.Markup.Standard
{
    public class TableTagFormatter : BaseFormatter
    {
        public class TableCell
        {
            public int Colspan { get; set; }
            public String Value { get; set; }
        }

        public class TableRow
        {
            private IList<TableCell> cells = new List<TableCell>();
            public IList<TableCell> TableCells
            {
                get { return this.cells; }
            }
        }

        private static Regex Table = new Regex(@"<table\s*(.*?)>(.*?)</table>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex TableRowPattern = new Regex(@"(?:\s|\n)*----+\r?\n(.*?)\r?\n(?:\s|\n)*----+\r?\n", RegexOptions.Singleline | RegexOptions.Compiled);
        private static String DefaultAttributes = @"class=""webscript-table"" cellspacing=""0"" cellpadding=""0""";

        private String attributes = DefaultAttributes; 
        
        #region IMarkupFormatter Members
        public override StringBuilder Convert(StringBuilder markup)
        {
            String html = Table.Replace(markup.ToString(), new MatchEvaluator(TableReferenceEvaluator));
            return new StringBuilder(html);
        }
        #endregion

        public String TableReferenceEvaluator(Match match)
        {
            String args = match.Groups[1].Value;
            String innerContent = match.Groups[2].Value;
            IList<TableRow> rows = Parse(innerContent);

            StringBuilder html = new StringBuilder();
            if (rows.Count == 0)
            {
                html.Append(match.Groups[0].Value);
            }
            else
            {
                if (String.IsNullOrWhiteSpace(args))
                    html.AppendLine("<table>");
                else
                    html.AppendFormat("<table {0}>",args).AppendLine();
                foreach (TableRow row in rows)
                {
                    html.Append("<tr>").AppendLine();
                    foreach (TableCell cell in row.TableCells)
                    {
                        String formatted = base.FormatEngine.Convert(cell.Value);
                        html.AppendFormat("<td colspan={0}>", cell.Colspan);
                        html.Append(formatted);
                        html.Append("</td>");
                    }
                    html.Append("</tr>").AppendLine();
                }
                html.Append("</table>").AppendLine();
            }

            return html.ToString();
        }

        /// <summary>
        /// Parses the table markup into rows and columns
        /// </summary>
        /// <param name="markup"></param>
        /// <returns></returns>
        internal IList<TableRow> Parse(String markup)
        {
            int maxcells = 1;

            IList<TableRow> rows = new List<TableRow>();

            Match match = TableRowPattern.Match(markup);
            while (match.Success)
            {
                TableRow row = new TableRow();
                String temp = match.Groups[1].Value.Trim();
                
                //Split the row into it's columns
                String[] cols = temp.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                
                if (cols.Length > maxcells)
                    maxcells = cols.Length;

                foreach (String col in cols)
                {
                    TableCell cell = new TableCell();
                    cell.Value = col.Trim();
                    cell.Colspan = 1;

                    row.TableCells.Add(cell);
                }

                rows.Add(row);
                match = match.NextMatch();
            }

            //We need to make a second pass to set the colspan for each of the cells
            foreach (TableRow row in rows)
            {
                int totalcells = row.TableCells.Count;
                if (totalcells < maxcells)
                    row.TableCells[totalcells - 1].Colspan = (maxcells - totalcells) + 1;
            }

            return rows;
        }
    }
}
