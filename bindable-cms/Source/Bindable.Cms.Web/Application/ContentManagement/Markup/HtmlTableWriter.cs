using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class HtmlTableWriter
    {
        private readonly List<string[]> _rows = new List<string[]>();

        public string Caption { get; set; }
        public string Index { get; set; }

        internal void WriteRow(string[] cells)
        {
            _rows.Add(cells.Select(cell => cell.Trim()).Select(cell => (cell == "-" ? "" : cell)).ToArray());
        }

        public override string ToString()
        {
            // Analyse the contents of each column to determine what kind of data it contains. This decides which CSS classes 
            // are applied to the column.
            var maxColumns = _rows.Max(row => row.Count());
            var classes = new string[maxColumns];
            for (var column = 0; column < maxColumns; column++)
            {
                var isDate = true;
                var isNumeric = true;

                for (var row = 1; row < _rows.Count; row++)
                {
                    if (column >= _rows[row].Length) continue;
                    
                    DateTime ignoreDate;
                    decimal ignoreNumber;
                    if (!DateTime.TryParse(_rows[row][column], out ignoreDate)) isDate = false;
                    if (!decimal.TryParse(_rows[row][column], out ignoreNumber)) isNumeric = false;
                }
                classes[column] = string.Empty;
                if (isDate) classes[column] += "date ";
                if (isNumeric) classes[column] += "numeric ";
                if (column == 0) classes[column] += "firstcolumn";
                classes[column] = classes[column].Trim();
            }

            // Render the table
            var output = new StringBuilder();
            output.AppendLine();
            output.AppendLine("<div class='table'>");
            output.AppendLine("<table>");
            output.AppendLine("<thead>");
            output.AppendLine("<tr>");
            for (var column = 0; column < maxColumns; column++)
            {
                var content = (column >= _rows[0].Length) ? "" : _rows[0][column];
                var cssClass = classes[column].Length == 0 ? "" : string.Format(" class='{0}'", classes[column]);
                output.AppendFormat("<td{0}>{1}</td>", cssClass, content);
            }
            output.AppendLine();
            output.AppendLine("</tr>");
            output.AppendLine("</thead>");
            output.AppendLine("<tbody>");
            for (var row = 1; row < _rows.Count - 1; row++ )
            {
                output.AppendLine("<tr>");
                for (var column = 0; column < maxColumns; column++)
                {
                    var content = (column >= _rows[row].Length) ? "" : _rows[row][column];
                    var cssClass = classes[column].Length == 0 ? "" : string.Format(" class='{0}'", classes[column]);
                    output.AppendFormat("<td{0}>{1}</td>", cssClass, content);
                }
                output.AppendLine();
                output.AppendLine("</tr>");
            }
            output.AppendLine("</tbody>");
            output.AppendLine("</table>");
            output.AppendLine("<div>");
            output.AppendFormat("<p>Table {0}: {1}</p>", Index, Caption).AppendLine();
            output.AppendLine("</div>");
            output.AppendLine("</div>");
            return output.ToString();
        }
    }
}