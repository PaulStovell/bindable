using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bindable.Core.CommandLine
{
    public class TableBuilder<T>
    {
        private readonly List<Column<T>> _columns = new List<Column<T>>();

        public TableBuilder()
        {
            ShowHeaders = true;
            HeaderForeground = ConsoleColor.Yellow;
            CellPadding = 4;
        }

        public bool ShowHeaders { get; set; }
        public ConsoleColor HeaderForeground { get; set; }
        public int CellPadding { get; set; }

        public void AddColumn(string header, Func<T, object> valueSelector)
        {
            AddColumn(header, valueSelector, "{0}");
        }

        public void AddColumn(string header, Func<T, object> valueSelector, string format)
        {
            AddColumn(header, valueSelector, format, 4);
        }

        public void AddColumn(string header, Func<T, object> valueSelector, string format, int minWidth)
        {
            AddColumn(header, valueSelector, format, minWidth, 60);
        }

        public void AddColumn(string header, Func<T, object> valueSelector, int minWidth)
        {
            AddColumn(header, valueSelector, minWidth, 60);
        }

        public void AddColumn(string header, Func<T, object> valueSelector, int minWidth, int maxWidth)
        {
            AddColumn(header, valueSelector, "{0}", minWidth, maxWidth);
        }

        public void AddColumn(string header, Func<T, object> valueSelector, string format, int minWidth, int maxWidth)
        {
            AddColumn(header, valueSelector, format, minWidth, maxWidth, null);
        }

        public void AddColumn(string header, Func<T, object> valueSelector, Func<T, ConsoleColor> foreground)
        {
            AddColumn(header, valueSelector, "{0}", 4, 60, foreground);
        }

        public void AddColumn(string header, Func<T, object> valueSelector, int minWidth, int maxWidth, Func<T, ConsoleColor> foreground)
        {
            AddColumn(header, valueSelector, "{0}", minWidth, maxWidth, foreground);
        }

        public void AddColumn(string header, Func<T, object> valueSelector, string format, int minWidth, int maxWidth, Func<T, ConsoleColor> foreground)
        {
            Columns.Add(new Column<T>()
                            {
                                Header = header, 
                                Value = valueSelector,
                                Format = format,
                                ValueColorer = foreground
                            });
        }

        public IList<Column<T>> Columns
        {
            get { return _columns; }
        }

        public void Render(IEnumerable<T> dataSource)
        {
            // Decide how wide each column should be based on all data in the rows
            foreach (var column in _columns)
            {
                column.PreferredWidth = Math.Max(dataSource.Select(row => column.Evaluate(row).Length / 4 + 1).Max() * 4, column.Header.Length);
            }
            
            // Render the table
            using (UltraConsole.Indent())
            {
                if (ShowHeaders)
                {
                    using (UltraConsole.ChangeColor(HeaderForeground))
                    {
                        foreach (var column in _columns)
                        {
                            UltraConsole.Write(column.Header.PadRight(column.PreferredWidth));
                            UltraConsole.Write("".PadRight(CellPadding, ' '));
                        }
                    }
                    UltraConsole.WriteLine();
                    foreach (var column in _columns)
                    {
                        for (var i = 0; i < column.PreferredWidth + 4; i++)
                        {
                            UltraConsole.Write("-");
                        }
                    }
                    UltraConsole.WriteLine();
                }

                foreach (var row in dataSource)
                {
                    foreach (var column in _columns)
                    {
                        var colorChange = null as IDisposable;
                        if (column.ValueColorer != null)
                        {
                            colorChange = UltraConsole.ChangeColor(column.ValueColorer(row));
                        }
                        UltraConsole.Write(column.Evaluate(row).PadRight(column.PreferredWidth));
                        UltraConsole.Write("".PadRight(CellPadding, ' '));
                        if (colorChange != null)
                        {
                            colorChange.Dispose();
                        }
                    }
                    UltraConsole.WriteLine();
                }
            }
        }
    }
}