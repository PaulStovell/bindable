using System;

namespace Bindable.Core.CommandLine
{
    public class Column<T>
    {
        public Column()
        {
            Header = string.Empty;
        }

        public string Header { get; set; }
        public string Format { get; set; }
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int PreferredWidth { get; set; }
        public Func<T, object> Value { get; set; }
        public ConsoleColor ValueColor { get; set; }
        public Func<T, ConsoleColor> ValueColorer { get; set; }

        public string Evaluate(T item)
        {
            var result = Value(item) ?? string.Empty;
            if (!string.IsNullOrEmpty(Format))
            {
                result = string.Format(Format, result);
            }
            return result.ToString().Trim();
        }
    }
}
